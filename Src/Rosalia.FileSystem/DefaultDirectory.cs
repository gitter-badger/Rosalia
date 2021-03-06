﻿namespace Rosalia.FileSystem
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    [DebuggerDisplay("{AbsolutePath}")]
    public class DefaultDirectory : IDirectory
    {
        public DefaultDirectory(string absolutePath) : base(absolutePath)
        {
        }

        public override bool Exists
        {
            get { return Directory.Exists(AbsolutePath); }
        }

        public override string Name
        {
            get { return Path.GetFileName(AbsolutePath); }
        }

        public override FileList Files
        {
            get
            {
                var files = Directory.GetFiles(AbsolutePath).Select(file => new DefaultFile(file));

                return new FileList(files, this);
            }
        }

        public override IEnumerable<IDirectory> Directories
        {
            get
            {
                return Directory
                    .GetDirectories(AbsolutePath)
                    .Select(file => new DefaultDirectory(file));
            }
        }

        public override IDirectory Parent
        {
            get
            {
                var parentDirectory = Directory.GetParent(AbsolutePath);
                if (parentDirectory == null)
                {
                    return null;
                }

                return new DefaultDirectory(parentDirectory.FullName);
            }
        }

        public override IDirectory GetDirectory(string name)
        {
            var path = Path.Combine(AbsolutePath, name);
            return new DefaultDirectory(path);
        }

        public override IFile GetFile(string name)
        {
            var path = Path.Combine(AbsolutePath, name);
            return new DefaultFile(path);
        }

        public override UnknownFileSystemItem this[string path]
        {
            get { return new UnknownFileSystemItem(this, path); }
        }

        public override void EnsureExists()
        {
            if (Exists)
            {
                return;
            }

            Directory.CreateDirectory(AbsolutePath);
        }

        public override void Delete()
        {
            Directory.Delete(AbsolutePath);
        }
    }
}