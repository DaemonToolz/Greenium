using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace GreeniumPrototype.Handler
{
    // Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
    //
    // Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

    public class DownloadHandler : IDownloadHandler {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownload(browser, downloadItem, callback, false);
        }

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback, bool UseDefault)
        {
            
            var defaultPath = $"{KnownFolders.GetPath(KnownFolder.Downloads)}/GreeniumDownload";
            if (!Directory.Exists(defaultPath))
                Directory.CreateDirectory(defaultPath);

            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            var filename = downloadItem.SuggestedFileName;
            if(filename.Contains("/") || filename.Contains("\\"))
                if (filename.Contains("/"))
                    filename = filename.Substring(filename.LastIndexOf("/"));
                if (filename.Contains("\\"))
                    filename = filename.Substring(filename.LastIndexOf("\\"));

            if (callback.IsDisposed) return;
            using (callback)
                if(UseDefault)
                    callback.Continue($"{defaultPath}/{filename}", showDialog: false); // /{downloadItem.SuggestedFileName}
                else
                    callback.Continue($"{downloadItem.SuggestedFileName}", showDialog: true); // /{downloadItem.SuggestedFileName}
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);
        }

    }
}
