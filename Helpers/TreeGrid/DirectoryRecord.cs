using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryView
{
    public class DirectoryRecord
    {
        #region Constructors

        public DirectoryRecord(DirectoryInfo info)
            : this(info, null)
        {
        }

        public DirectoryRecord(DirectoryInfo info, DirectoryRecord parent)
        {
            this.parent = parent;
            this.Info = info;
        }

        #endregion

        #region Information

        public DirectoryInfo Info { get; private set; }

        private List<DirectoryRecord> items;
        public List<DirectoryRecord> Items
        {
            get
            {
                if (items == null)
                    items = (from di in Info.EnumerateDirectories()
                             select new DirectoryRecord(di, this)).ToList();
                return items;
            }
        }

        private IEnumerable<FileInfo> files;
        public IEnumerable<FileInfo> Files
        {
            get {
                if (files == null)
                    files = Info.EnumerateFiles();
                return files; 
            }
        }

        #endregion

        #region Level

        // Returns the number of nodes in the longest path to a leaf
        
        public int Depth
        {
            get
            {
                int max;
                if (Items.Count == 0)
                    max = 0;
                else
                    max = (int)Items.Max(r => r.Depth);
                return max + 1;
            }
        }

        private DirectoryRecord parent;

        // Returns the maximum depth of all siblings

        public int Level
        {
            get
            {
                if (parent == null)
                    return Depth;
                return parent.Level-1;
            }
        }

        #endregion
    }
}
