<%@ Page Title="xAPI AU Simulator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CMI5_AU._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Title %></h2>

    <div class="panel panel-default col-md-12">
        <div class="panel-body">
            <h3>Hit these buttons in sequence to view interactively or hit the sequencer button</h3>
            <table class="table">
                <tr>
                    <th class="col-md-4">Action</th>
                    <th class="col-md-8">Outcome</th>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Launched" CssClass="btn btn-default" Text="Send the launched statement" OnClick="Launched_Click" /></td>
                    <td>Launched</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Initialized" CssClass="btn btn-default" Text="Send initialised statement" OnClick="Initialized_Click" /></td>
                    <td>Initialized</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Progressed" CssClass="btn btn-default" Text="Send a Progressed statement" OnClick="Progressed_Click"/></td>
                    <td>Sends progressed 100%</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Completed" CssClass="btn btn-default" Text="Send completed statement" OnClick="Completed_Click" /></td>
                    <td>Sends the complete statement</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Passed" CssClass="btn btn-default" Text="Send passed statement" OnClick="Passed_Click" /></td>
                    <td>Sends a passed statement (Above set mastery score)</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="Terminated" CssClass="btn btn-default" Text="Send terminated statement" OnClick="Terminated_Click" /></td>
                    <td>Sends a terminated statement (Session dead after this?)</td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" ID="AllButtons" CssClass="btn btn-danger" Text="Sequencer" OnClick="AllButtons_Click" /></td>
                    <td>Send all the above statments in order</td>
                </tr>

            </table>
        </div>
    </div>


</asp:Content>
