<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EMSPushMessageDemo.aspx.cs" Inherits="PushNotificationWebAp.EMSPushMessageDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Text="URI："></asp:Label><asp:TextBox ID="txtURI" runat="server" Width="808px" ReadOnly="True"></asp:TextBox>
        <br/>
        <asp:Label ID="Label2" runat="server" Text="username："></asp:Label><asp:TextBox ID="txtUserName" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="password："></asp:Label><asp:TextBox ID="txtPassword" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Receiver："></asp:Label><asp:TextBox ID="txtReceiverID" runat="server" Width="926px">A123456789</asp:TextBox>
        <br/>
        <asp:Label ID="Label5" runat="server" Text="主旨："></asp:Label><asp:TextBox ID="txtOTATopicName" runat="server" Width="566px">貸款申請核准通知</asp:TextBox>
        <br/>
        <asp:Label ID="Label6" runat="server" Text="Message："></asp:Label><asp:TextBox ID="txtMessage" runat="server" Rows="10" TextMode="MultiLine" Width="746px"></asp:TextBox>
        <br />
		<asp:Label ID="Label8" runat="server" Text="附件："></asp:Label><asp:FileUpload ID="FileUpload1" runat="server" Width="286px" AllowMultiple="True" />
		<br />
        <asp:Label ID="Label7" runat="server" Text="發送筆數："></asp:Label><asp:TextBox ID="txtMessageNums" runat="server" Width="102px">1</asp:TextBox>
        <br />
        <asp:Button ID="btn_Send" runat="server" Text="發送訊息" OnClick="btnPushMessage_Click" />
    </div>
    </form>
</body>
</html>
