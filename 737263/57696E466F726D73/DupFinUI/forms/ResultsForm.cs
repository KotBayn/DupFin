using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DupFin.Services;

namespace DupFinUI.forms
{
    public partial class ResultsForm : Form
    {
        private bool _checkPaths;

        public ResultsForm(bool checkPaths)
        {
            InitializeComponent();
            _checkPaths = checkPaths;
            LoadResults();
        }

        private void LoadResults()
        {
            int duplicatesCount = 0;
            treeResults.Nodes.Clear();

            foreach (var group in FileScanner.FoundFiles)
            {
                string[] files = group.Value.OrderBy(f => f).ToArray(); // sort files alphabetically

                if (files.Length > 1)
                {
                    duplicatesCount += (files.Length - 1);

                    // If the key contains the file name (Strict Match enabled), clean up the display
                    string displayHash = group.Key.Length > 8 ? group.Key.Substring(0, 8) : group.Key;
                    TreeNode groupNode = new TreeNode($"Group Hash: {displayHash}... ({files.Length} files)");

                    if (_checkPaths)
                    {
                        var folders = files.GroupBy(f => System.IO.Path.GetDirectoryName(f));
                        foreach (var folder in folders)
                        {
                            TreeNode folderNode = new TreeNode($"📁 {folder.Key}");
                            foreach (var file in folder)
                            {
                                // Important: store the full path in the Tag so the Delete button knows what to delete
                                TreeNode fileNode = new TreeNode(System.IO.Path.GetFileName(file)) { Tag = file };
                                folderNode.Nodes.Add(fileNode);
                            }
                            groupNode.Nodes.Add(folderNode);
                        }
                    }
                    else
                    {
                        // Standard flat output
                        foreach (string file in files)
                        {
                            groupNode.Nodes.Add(new TreeNode(file) { Tag = file });
                        }
                    }

                    treeResults.Nodes.Add(groupNode);
                }
            }

            labelCount.Text = $"Total duplicates found: {duplicatesCount}";
            // Do not expand the tree automatically if complex grouping is enabled to avoid lag
            if (!_checkPaths) treeResults.ExpandAll();
        }
        private void treeResults_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // ButPlug
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var filesToDelete = new List<TreeNode>();

            // Recursive search for all checked checkboxes in the tree
            CollectCheckedNodes(treeResults.Nodes, filesToDelete);

            if (filesToDelete.Count == 0)
            {
                MessageBox.Show("Select at least one file to delete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var warning = MessageBox.Show($"Are you sure you want to delete {filesToDelete.Count} files?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warning == DialogResult.No) return;

            int deletedCount = 0;
            foreach (var node in filesToDelete)
            {
                try
                {
                    string filePath = node.Tag.ToString();
                    System.IO.File.Delete(filePath);
                    node.Remove();
                    deletedCount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not delete {node.Text}\nReason: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            CleanUpEmptyNodes();
            MessageBox.Show($"Successfully deleted {deletedCount} files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Recursive function to collect checked nodes
        private void CollectCheckedNodes(TreeNodeCollection nodes, List<TreeNode> list)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null && node.Checked)
                {
                    list.Add(node);
                }
                CollectCheckedNodes(node.Nodes, list);
            }
        }

        // Delete empty folders
        private void CleanUpEmptyNodes()
        {
            for (int i = treeResults.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode hashNode = treeResults.Nodes[i];

                for (int j = hashNode.Nodes.Count - 1; j >= 0; j--)
                {
                    if (hashNode.Nodes[j].Tag == null && hashNode.Nodes[j].Nodes.Count == 0)
                    {
                        hashNode.Nodes[j].Remove();
                    }
                }

                // If in whole groupe is left 1 file
                int totalFilesLeft = GetFileCount(hashNode);
                if (totalFilesLeft <= 1)
                {
                    hashNode.Remove();
                }
            }
        }

        private int GetFileCount(TreeNode node)
        {
            if (node.Tag != null) return 1;
            int count = 0;
            foreach (TreeNode child in node.Nodes) count += GetFileCount(child);
            return count;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Return to the main form
            Application.Restart();
            Environment.Exit(0);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if there are actually any results to save
            if (FileScanner.FoundFiles.Count == 0)
            {
                MessageBox.Show("No duplicates found to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Open the standard Windows save file dialog
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.Title = "Save Scan Results";
                saveDialog.FileName = $"DupFin_Results_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveResultsToFile(saveDialog.FileName);
                }
            }
        }
        // The logic we ported from the old ChoiceService.cs
        private void SaveResultsToFile(string filePath)
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("*===* DupFin Scan Results *===*");
                sb.AppendLine($"Date: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
                sb.AppendLine();

                int totalGroups = 0;

                foreach (var group in FileScanner.FoundFiles)
                {
                    string[] files = group.Value.ToArray();
                    if (files.Length > 1)
                    {
                        totalGroups++;
                        sb.AppendLine($"[Hash: {group.Key}]");

                        foreach (string file in files)
                        {
                            sb.AppendLine($"  + {file}");
                        }
                        sb.AppendLine();
                    }
                }

                sb.AppendLine($"Total duplicate groups: {totalGroups}");

                System.IO.File.WriteAllText(filePath, sb.ToString());
                MessageBox.Show("Results saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var filesToDelete = new System.Collections.Generic.List<TreeNode>();
            bool isDeletingEntireGroup = false;

            // Iterate through parent nodes (Hash Groups)
            foreach (TreeNode groupNode in treeResults.Nodes)
            {
                int checkedCount = 0;

                // First pass: count how many files in this specific group are checked
                foreach (TreeNode fileNode in groupNode.Nodes)
                {
                    if (fileNode.Checked) checkedCount++;
                }

                // FOOLPROOF CHECK: Did the user check ALL files in this group?
                if (checkedCount == groupNode.Nodes.Count && checkedCount > 0)
                {
                    isDeletingEntireGroup = true;
                }

                // Second pass: add checked files to our death row list
                foreach (TreeNode fileNode in groupNode.Nodes)
                {
                    if (fileNode.Checked) filesToDelete.Add(fileNode);
                }
            }

            // If nothing is selected, exit
            if (filesToDelete.Count == 0)
            {
                MessageBox.Show("Select at least one file to delete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // If user is about to wipe out all copies of a file
            if (isDeletingEntireGroup)
            {
                var criticalWarning = MessageBox.Show(
                    "DANGER! You have selected ALL copies in one or more groups." +
                    "\nIf you proceed, you will completely lose these files (no originals kept).\n" +
                    "\nAre you absolutely sure you want to proceed?",
                    "CRITICAL WARNING",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop); // Stop icon 

                if (criticalWarning == DialogResult.No) return; // abort
            }
            else
            {
                // Standard warning if they are keeping at least one original
                var standardWarning = MessageBox.Show(
                    $"Are you sure you want to delete {filesToDelete.Count} duplicate files?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (standardWarning == DialogResult.No) return;
            }

            // The Order 66
            int deletedCount = 0;

            foreach (var node in filesToDelete)
            {
                try
                {
                    // node.Text contains the full path to the file
                    System.IO.File.Delete(node.Text);

                    // Remove the node from the UI tree
                    node.Remove();

                    deletedCount++;
                }
                catch (Exception ex)
                {
                    // If a file is locked or requires admin rights (like C:\Windows), it fails gracefully here
                    MessageBox.Show($"Could not delete {node.Text}\nReason: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Cleanup the TreeView: Remove empty parent groups
            for (int i = treeResults.Nodes.Count - 1; i >= 0; i--)
            {
                if (treeResults.Nodes[i].Nodes.Count <= 1)
                {
                    // If a group has 0 or 1 file left, it's no longer a "duplicate group", so remove it from view
                    treeResults.Nodes[i].Remove();
                }
            }

            MessageBox.Show($"Successfully deleted {deletedCount} files. Disk space freed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Nuke
            if (e.CloseReason == CloseReason.UserClosing) Environment.Exit(0);
        }
    }
}