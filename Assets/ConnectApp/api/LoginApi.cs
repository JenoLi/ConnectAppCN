using System;
using System.Collections;
using System.Text;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class LoginApi {
        public static IPromise<LoginInfo> LoginByEmail(string email, string password) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<LoginInfo>();
            Window.instance.startCoroutine(_LoginByEmail(promise, email, password));
            return promise;
        }

        private static IEnumerator _LoginByEmail(IPendingPromise<LoginInfo> promise, string email, string password) {
            var para = new LoginParameter {
                email = email,
                password = password
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(IApi.apiAddress + "/auth/live/login", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
#pragma warning disable 618
            yield return request.Send();
#pragma warning restore 618

            if (request.isNetworkError) {
                // something went wrong
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) {
                // or the response is not OK
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
//                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE"))
//                    cookie = request.GetResponseHeaders()["SET-COOKIE"];
                // Format output and resolve promise
                var json = request.downloadHandler.text;
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
                if (loginInfo != null)
                    promise.Resolve(loginInfo);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}