using System;
using System.Collections.Generic;
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
            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            if (callback.IsDisposed) return;
            using (callback)
                callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
            
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);
        }

    }
}
