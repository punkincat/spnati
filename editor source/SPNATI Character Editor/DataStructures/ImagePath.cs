using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SPNATI_Character_Editor.DataStructures
{

    public class ImagePath
    {
        string _baseName;

        /// <summary>
        /// Names the base folder or path for this image. <br/> <br/>
        /// 
        /// For paths with implied directories, this is null. <br/>
        /// 
        /// For paths that explicitly name folders in opponents/ or opponents/reskins/, this is everything up to and including the character or costume folder name (e.g. `opponents/mari`, `opponents/reskins/mari_summer`, etc). <br/>
        /// 
        /// For other paths relative to the repository root, this is everything up to the filename (e.g. `img`, `img/backgrounds`, etc).
        /// In particular, an empty string indicates a file directly inside the repository root folder. <br/>
        /// <br/>
        /// In all cases, this name neither starts nor ends with a path separator (`opponents/mari`, not `opponents/mari/`).
        /// </summary>
        public string BaseName { get { return _baseName; } }

        string _subFolder;

        /// <summary>
        /// Names the portion of the path between the BaseName and the FileName, if any. <br/>
        /// For paths to files not under `opponents/` or `opponents/reskins/`, this will be an empty string.
        /// </summary>
        public string SubFolder { get { return _subFolder ?? ""; } }

        /// <summary>
        /// The filename for this image.
        /// </summary>
        string _fileName;

        public string FileName { get { return _fileName ?? ""; } }

        private ImagePath(string baseName, string subFolder, string fileName)
        {
            this._baseName = baseName;
            this._subFolder = subFolder;
            this._fileName = fileName;

            Debug.Assert(String.IsNullOrEmpty(this._baseName) || (!this._baseName.Contains('\\') && !this._baseName.StartsWith("/") && !this._baseName.EndsWith("/")));
            Debug.Assert(String.IsNullOrEmpty(this._subFolder) || (!this._subFolder.Contains('\\') && !this._subFolder.StartsWith("/") && !this._subFolder.EndsWith("/")));
            Debug.Assert(!String.IsNullOrEmpty(this._fileName) && !this._fileName.Contains('\\') && !this._fileName.StartsWith("/") && !this._fileName.EndsWith("/"));
        }

        /// <summary>
        /// Get a path string suitable for use as a Pose Maker sprite source.
        /// </summary>
        /// <returns></returns>
        public string AsPoseSpriteSrc(ISkin skin)
        {
            StringBuilder sb = new StringBuilder();

            string effectiveBaseName = this._baseName;
            string skinFolder = "opponents/";
            if (skin is Costume)
            {
                skinFolder += "reskins/";
            }
            skinFolder += skin.FolderName;

            if (effectiveBaseName == skinFolder)
            {
                effectiveBaseName = null;                
            }

            if (effectiveBaseName != null)
            {
                if (!effectiveBaseName.StartsWith("opponents"))
                {
                    sb.Append('/');
                }
                sb.Append(effectiveBaseName);
                sb.Append('/');
            }

            if (!String.IsNullOrWhiteSpace(this._subFolder))
            {
                sb.Append(this._subFolder);
                sb.Append('/');
            }

            sb.Append(this._fileName);
            return sb.ToString();
        }

        /// <summary>
        /// Get a path string suitable for use as an epilogue directive source.
        /// </summary>
        /// <returns></returns>
        public string AsEpilogueSrc(Character character)
        {
            StringBuilder sb = new StringBuilder();

            if (this._baseName != null && this._baseName != "opponents/" + character.FolderName)
            {
                sb.Append('/');
                sb.Append(this._baseName);
                sb.Append('/');
            }

            if (!String.IsNullOrWhiteSpace(this._subFolder))
            {
                sb.Append(this._subFolder);
                sb.Append('/');
            }

            sb.Append(this._fileName);
            return sb.ToString();
        }

        /// <summary>
        /// Get a path string that can be used for filesystem access and is relative to the SPNATI directory.
        /// </summary>
        /// <param name="skin">A skin from which to derive folders for implicitly-defined paths.</param>
        /// <param name="stage">An optional stage to use when deriving implicitly-defined paths.</param>
        /// <returns></returns>
        public string AsRelativeFilesystemPath(ISkin skin, int? stage = null)
        {
            StringBuilder sb = new StringBuilder();
            if (this._baseName == null)
            {
                if (skin is Costume)
                {
                    if (stage != null)
                    {
                        Costume costume = (Costume)skin;
                        string folder = "opponents/reskins/" + skin.FolderName;
                        int bestFitStage = -1;
                        int targetStage = stage ?? 0;

                        foreach (StageSpecificValue val in costume.Folders)
                        {
                            if (val.Stage > bestFitStage && val.Stage <= targetStage)
                            {
                                bestFitStage = val.Stage;
                                folder = val.Value;
                            }
                        }

                        foreach (string part in SplitParts(folder))
                        {
                            if (sb.Length > 0)
                            {
                                sb.Append(Path.DirectorySeparatorChar);
                            }
                            sb.Append(part);
                        }
                    }
                    else
                    {
                        sb.Append("opponents");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("reskins");
                        sb.Append(skin.FolderName);
                    }
                }
                else
                {
                    sb.Append("opponents");
                    sb.Append(Path.DirectorySeparatorChar);
                    sb.Append(skin.FolderName);
                }

                sb.Append(Path.DirectorySeparatorChar);
            }
            else if (!String.IsNullOrWhiteSpace(this._baseName))
            {
                sb.Append(this._baseName);
                sb.Append(Path.DirectorySeparatorChar);
            }

            if (!String.IsNullOrWhiteSpace(this._subFolder))
            {
                sb.Append(this._subFolder);
                sb.Append(Path.DirectorySeparatorChar);
            }

            sb.Append(this._fileName);
            return sb.ToString();
        }

        /// <summary>
        /// Get a path string that can be used for filesystem access and is absolute.
        /// </summary>
        /// <param name="skin">A skin from which to derive folders for implicitly-defined paths.</param>
        /// <param name="stage">An optional stage to use when deriving implicitly-defined paths.</param>
        /// <returns></returns>
        public string AsAbsoluteFilesystemPath(ISkin skin, int? stage = null)
        {
            return Path.Combine(Config.SpnatiDirectory, this.AsRelativeFilesystemPath(skin, stage))
        }

        /// <summary>
        /// Parse a sprite path from a raw local filesystem path, for example one obtained from a file picker dialog. <br/>
        /// If the supplied path is relative, it is assumed to be relative to the SPNATI directory.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <returns>The parsed ImagePath, or null if the path is empty, does not name a path within the SPNATI directory, or is otherwise invalid.</returns>
        public static ImagePath ParseFileSystemPath(string path)
        {
            string baseName = "";
            string subFolder = "";
            string fileName = "";

            if (Path.IsPathRooted(path))
            {
                string spnatiDir = Path.GetFullPath(Config.SpnatiDirectory);

                path = Path.GetFullPath(path);
                if (path.StartsWith(spnatiDir))
                {
                    path = path.Substring(spnatiDir.Length);
                }
                else
                {
                    return null;
                }
            }

            Queue<string> parts = SplitParts(path);
            if (parts.Count >= 1 && String.IsNullOrWhiteSpace(parts.Peek()))
            {
                parts.Dequeue();
            }

            if (parts.Count == 0)
            {
                return null;
            }

            if (parts.Count > 1)
            {
                if (parts.Peek() == "opponents")
                {
                    baseName = parts.Dequeue();

                    // Check if we're pointing to a costume
                    if (parts.Count > 1 && parts.Peek() == "reskins")
                    {
                        baseName += "/" + parts.Dequeue();
                    }

                    // Get the costume name if possible
                    if (parts.Count > 1)
                    {
                        baseName += "/" + parts.Dequeue();
                    }

                    // Extract remaining subfolder paths
                    while (parts.Count > 1)
                    {
                        subFolder = (String.IsNullOrEmpty(subFolder) ? "" : subFolder + "/") + parts.Dequeue();
                    }
                }
                else
                {
                    // Get path into non-opponent folder
                    while (parts.Count > 1)
                    {
                        baseName = (String.IsNullOrEmpty(baseName) ? "" : baseName + "/") + parts.Dequeue();
                    }
                }
            }

            Debug.Assert(parts.Count == 1);
            fileName = parts.Dequeue();

            return new ImagePath(baseName, subFolder, fileName);
        }

        /// <summary>
        /// Parse a sprite path as used in a Pose Maker pose. <br/>
        /// Within SPNATI, this roughly corresponds to the parsing logic in `getActualSpriteSrc`.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <param name="skin">The base skin that this path is being loaded from.</param>
        /// <returns>The parsed ImagePath, or null if the path is empty and/or invalid.</returns>
        public static ImagePath ParsePoseSpriteSrc(string path, ISkin skin)
        {
            Queue<string> parts = SplitParts(path);
            string baseName = null;
            string subFolder = null;
            string fileName = null;

            if (parts.Count == 0)
            {
                return null;
            }

            if (parts.Count > 1)
            {
                if (String.IsNullOrWhiteSpace(parts.Peek()))
                {
                    while (parts.Count > 1)
                    {
                        baseName = (String.IsNullOrEmpty(baseName) ? "" : baseName + "/") + parts.Dequeue();
                    }
                }
                else if (parts.Peek() == skin.Character.FolderName)
                {
                    baseName = "opponents/" + parts.Dequeue();
                }
                else if (parts.Peek() == "reskins" || parts.Peek() == "opponents")
                {
                    if (parts.Peek() == "opponents")
                    {
                        baseName = parts.Dequeue();
                    }
                    else
                    {
                        baseName = "opponents";
                    }

                    if (parts.Count > 1 && parts.Peek() == "reskins")
                    {
                        baseName += "/" + parts.Dequeue();
                    }

                    if (parts.Count > 1)
                    {
                        baseName += "/" + parts.Dequeue();
                    }
                }
                else
                {
                    baseName = null;
                }

                Debug.Assert(parts.Count >= 1);

                while (parts.Count > 1)
                {
                    subFolder = (String.IsNullOrEmpty(subFolder) ? "" : subFolder + "/") + parts.Dequeue();
                }
            }

            Debug.Assert(parts.Count == 1);
            fileName = parts.Dequeue();

            return new ImagePath(baseName, subFolder, fileName);
        }

        /// <summary>
        /// Parse a sprite path as used in an epilogue directive. <br/>
        /// Within SPNATI, this roughly corresponds to the parsing logic in `EpiloguePlayer.prototype.load`.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <returns>The parsed ImagePath, or null if the path is empty and/or invalid.</returns>
        public static ImagePath ParseEpilogueSpriteSrc(string path)
        {
            Queue<string> parts = SplitParts(path);
            string baseName = null;
            string subFolder = null;
            string fileName = null;

            if (parts.Count == 0)
            {
                return null;
            }

            if (parts.Count > 1)
            {
                if (String.IsNullOrWhiteSpace(parts.Peek()))
                {
                    baseName = parts.Dequeue();

                    if (parts.Count > 1)
                    {
                        if (parts.Peek() == "opponents")
                        {
                            baseName = parts.Dequeue();

                            if (parts.Count > 1 && parts.Peek() == "reskins")
                            {
                                baseName += "/" + parts.Dequeue();
                            }

                            if (parts.Count > 1)
                            {
                                baseName += "/" + parts.Dequeue();
                            }

                            while (parts.Count > 1)
                            {
                                subFolder = (String.IsNullOrEmpty(subFolder) ? "" : subFolder + "/") + parts.Dequeue();
                            }
                        }
                        else
                        {
                            while (parts.Count > 1)
                            {
                                baseName = (String.IsNullOrEmpty(baseName) ? "" : baseName + "/") + parts.Dequeue();
                            }
                        }
                    }


                    while (parts.Count > 1)
                    {
                        baseName = (String.IsNullOrEmpty(baseName) ? "" : baseName + "/") + parts.Dequeue();
                    }
                }
                else
                {
                    baseName = null;
                }
            }

            Debug.Assert(parts.Count >= 1);
            while (parts.Count > 1)
            {
                subFolder = (String.IsNullOrEmpty(subFolder) ? "" : subFolder + "/") + parts.Dequeue();
            }

            Debug.Assert(parts.Count == 1);
            fileName = parts.Dequeue();

            return new ImagePath(baseName, subFolder, fileName);
        }

        /// <summary>
        /// Split a path into a list of whitespace-trimmed segments. <br/>
        /// This method filters out most empty segments in the path, unless an empty segment comes at the start of the path, which indicates that it is an absolute path.
        /// </summary>
        /// <param name="path">The path to split.</param>
        /// <returns>A list of trimmed segments in this path.</returns>
        private static Queue<string> SplitParts(string path)
        {
            Queue<string> ret = new Queue<string>();
            foreach (string part in path.Split('/', '\\'))
            {
                string trimmed = part.Trim();
                if (!String.IsNullOrEmpty(trimmed) || ret.Count == 0)
                {
                    ret.Append(trimmed);
                }
            }
            return ret;
        }
    }
}
