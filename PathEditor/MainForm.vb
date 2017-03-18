Imports System.IO
Imports System.Security
Public Class MainForm
    Sub New()
        InitializeComponent()
        GetPathVariable()
    End Sub

    Private Sub GetPathVariable()
        Dim envirValue As String = String.Empty
        Try
            envirValue = Environment.GetEnvironmentVariable("Path", Target)
        Catch generatedExceptionName As SecurityException
            MessageBox.Show(generatedExceptionName.ToString)
        End Try
        If envirValue <> "" Then
            Dim Item As String() = envirValue.Split(New Char() {Path.PathSeparator}, StringSplitOptions.RemoveEmptyEntries)
            lb.Items.Clear()
            lb.Items.AddRange(Item)
            lb.SelectedIndex = 0
        End If
    End Sub


    Private Sub SetPathVariable()
        Dim envirValue As String = BuildPathVariable()
        Try
            Environment.SetEnvironmentVariable("Path", envirValue, Target)
        Catch generatedExceptionName As SecurityException
            MessageBox.Show(generatedExceptionName.ToString)
        End Try
    End Sub

    Private Function BuildPathVariable() As String
        Dim envirValue As String = [String].Empty

        For Each o As [Object] In lb.Items
            envirValue += o.ToString() + Path.PathSeparator
        Next

        Return envirValue
    End Function

    Public ReadOnly Property Target() As EnvironmentVariableTarget
        Get
            If radSystem.Checked Then
                Return EnvironmentVariableTarget.Machine
            ElseIf radProcess.Checked Then
                Return EnvironmentVariableTarget.Process
            Else
                Return EnvironmentVariableTarget.User
            End If
        End Get
    End Property

#Region "Control Event"

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        If fbd.ShowDialog() = DialogResult.OK Then
            txtPath.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not [String].IsNullOrEmpty(txtPath.Text) Then
            lb.Items.Add(txtPath.Text)
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim idx As Integer = lb.SelectedIndex
        If idx <> -1 Then
            lb.Items.RemoveAt(idx)
            idx -= 1
            If idx < 0 Then
                idx = 0
            End If
            lb.SelectedIndex = idx
        End If
    End Sub

    Private Sub lb_MouseUp(sender As Object, e As MouseEventArgs) Handles lb.MouseUp
        txtPath.Text = lb.SelectedItem.ToString()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim idx As Integer = lb.SelectedIndex
        If idx <> -1 Then
            lb.Items(idx) = txtPath.Text
        End If
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SetPathVariable()
        Close()
    End Sub

    Private Sub CheckedChanged(sender As Object, e As EventArgs) Handles radProcess.CheckedChanged, radSystem.CheckedChanged, radUser.CheckedChanged
        GetPathVariable()
        txtPath.Text = lb.SelectedItem.ToString()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Text files (*.txt)|*.txt"
            sfd.Title = "Save Environment Variable to Disk"
            sfd.FileName = "OldPathValue.txt"
            sfd.DefaultExt = ".txt"
            If sfd.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(sfd.FileName, BuildPathVariable())
            End If
        End Using
    End Sub
#End Region

End Class
