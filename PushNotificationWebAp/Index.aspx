﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="PushNotificationWebAp.Index1" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>您的 ASP.NET 應用程式</title>
    <style>
        body {
            background: #fff;
            color: #505050;
            font: 14px 'Segoe UI', tahoma, arial, helvetica, sans-serif;
            margin: 20px;
            padding: 0;
        }

        #header {
            background: #efefef;
            padding: 0;
        }

        h1 {
            font-size: 48px;
            font-weight: normal;
            margin: 0;
            padding: 0 30px;
            line-height: 150px;
        }

        p {
            font-size: 20px;
            color: #fff;
            background: #969696;
            padding: 0 30px;
            line-height: 50px;
        }

        #main {
            padding: 5px 30px;
        }

        .section {
            width: 21.7%;
            float: left;
            margin: 0 0 0 4%;
        }

            .section h2 {
                font-size: 13px;
                text-transform: uppercase;
                margin: 0;
                border-bottom: 1px solid silver;
                padding-bottom: 12px;
                margin-bottom: 8px;
            }

            .section.first {
                margin-left: 0;
            }

                .section.first h2 {
                    font-size: 24px;
                    text-transform: none;
                    margin-bottom: 25px;
                    border: none;
                }

                .section.first li {
                    border-top: 1px solid silver;
                    padding: 8px 0;
                }

            .section.last {
                margin-right: 0;
            }

        ul {
            list-style: none;
            padding: 0;
            margin: 0;
            line-height: 20px;
        }

        li {
            padding: 4px 0;
        }

        a {
            color: #267cb2;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            <div id="header">
                <h1>您的 ASP.NET 應用程式</h1>
                <p>恭喜! 您已建立了一個專案</p>
            </div>

            <div id="main">
                <div class="section first">
                    <h2>此應用程式的組成項目:</h2>
                    <ul>
                        <li>範例頁面顯示首頁、關於和連絡人間的導覽。</li>
                        <li>使用 <a href="http://go.microsoft.com/fwlink/?LinkID=615519">Bootstrap</a> 進行佈景主題</li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615520">驗證</a>若已選取，顯示如何註冊並登入</li>
                        <li>使用 <a href="http://go.microsoft.com/fwlink/?LinkID=615521">NuGet</a> 管理 ASP.NET 功能</li>
                    </ul>
                </div>

                <div class="section">
                    <h2>自訂應用程式</h2>
                    <ul>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615522">開始使用 ASP.NET Web 論壇</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615523">變更網站的佈景主題</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615524">使用 NuGet 新增更多程式庫</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615525">設定驗證</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615526">自訂網站使用者的資訊</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615527">從社交提供者取得資訊</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615528">使用 ASP.NET Web API 新增 HTTP 服務</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615529">保護 Web API</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615530">透過 ASP.NET SignalR 新增即時 Web</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615531">使用 Scaffolding 新增元件</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615532">透過瀏覽器連結測試應用程式</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615533">共用您的專案</a></li>
                    </ul>
                </div>

                <div class="section">
                    <h2>部署</h2>
                    <ul>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615534">確保您的應用程式可供生產</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615535">Microsoft Azure</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615536">主控提供者</a></li>
                    </ul>
                </div>

                <div class="section last">
                    <h2>取得說明</h2>
                    <ul>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615537">取得說明</a></li>
                        <li><a href="http://go.microsoft.com/fwlink/?LinkID=615538">取得更多範本</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
