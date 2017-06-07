using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.Data.SmallFTPUploader.Functions
{
    internal static class PathTranslations
    {
        /// <summary>
        /// Translate the localpath to remotepath.
        /// </summary>
        /// <param name="paramFilePath">Full filepath of this file</param>
        /// <param name="paramInputPath">Start und input path</param>
        /// <param name="paramRemotePath">Current Remotepath</param>
        /// <returns>Translatet remotepath</returns>
        internal static string TranslateLocalPathToRemote(string paramFilePath, string paramInputPath, string paramRemotePath)
        {
            if (!paramInputPath.EndsWith("\\"))
            {
                paramInputPath = paramInputPath + "\\";
            }
            var returnPath = paramFilePath.Replace(paramInputPath, paramRemotePath);
            returnPath = returnPath.Replace("\\", "/");

            return returnPath;
        }
    }
}
