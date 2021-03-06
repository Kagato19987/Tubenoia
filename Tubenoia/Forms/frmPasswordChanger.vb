﻿Imports System.Text
Imports ProjectWIFE
Public Class frmPasswordChanger
    Private INI As New INI(Configuration.INI_Configuration.DefaultConfig, My.Application.Info.DirectoryPath & "\config.ini")
    Dim MaxThread As Integer
    Dim F As New Functions.General
    Private Sub frmAccountChecker_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Password.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub
    Private Sub frmAccountChecker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Userlist.ContextMenu = CM
        lstPasswords.ContextMenu = CMp
        lstNew.ContextMenu = CMn
        CheckForIllegalCrossThreadCalls = False
        MaxThread = CInt(INI("General")("Threads").Value)
        If MaxThread > 50 Then
            MaxThread = 50
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnMinimize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub Userlist_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Userlist.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub
    Private Sub Userlist_DragDrop(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.DragEventArgs) Handles Userlist.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim MyFiles() As String
            Dim i As Integer
            MyFiles = e.Data.GetData(DataFormats.FileDrop)
            Dim t As Integer
            For i = 0 To MyFiles.Length - 1
                Dim Users = F.Lines(IO.File.ReadAllText(MyFiles(i)))
                Dim Buffer As String()
                For o = 0 To Users.Length - 1
                    If Users(o).Contains(":") Then
                        Buffer = Users(o).Split(":")
                        If UBound(Buffer) = 5 Then
                            Dim T2 As New ListViewItem
                            T2.Text = Buffer(0)
                            T2.SubItems.Add(Buffer(1))
                            T2.SubItems.Add(Buffer(2))
                            T2.SubItems.Add(Buffer(3))
                            T2.SubItems.Add(Buffer(4))
                            T2.SubItems.Add(Buffer(5))
                            Userlist.Items.Add(T2)
                        ElseIf UBound(Buffer) = 3 Then
                            Dim T2 As New ListViewItem
                            T2.Text = Buffer(0)
                            T2.SubItems.Add(Buffer(1))
                            T2.SubItems.Add(Buffer(2))
                            T2.SubItems.Add(Buffer(3))
                            Userlist.Items.Add(T2)
                        Else
                            Userlist.Items.Add(Buffer(0)).SubItems.Add(Buffer(1))
                        End If
                        t += 1
                    End If
                Next
                Usercount.Text = "TOTAL USERS: " & Userlist.Items.Count
                Me.Refresh()
            Next
        End If
    End Sub
    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Userlist.Items.Clear()
        Usercount.Text = "TOTAL USERS: " & Userlist.Items.Count
    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        For Each itm As ListViewItem In Userlist.SelectedItems
            itm.Remove()
        Next
        Usercount.Text = "TOTAL USERS: " & Userlist.Items.Count
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        Using T As New OpenFileDialog
            If T.ShowDialog = DialogResult.OK Then
                Dim Users = F.Lines(IO.File.ReadAllText(T.FileName))
                Dim Buffer As String()
                For I = 0 To Users.Length - 1
                    If Users(I).Contains(":") Then
                        Buffer = Users(I).Split(":")
                        If UBound(Buffer) = 5 Then
                            Dim T2 As New ListViewItem
                            T2.Text = Buffer(0)
                            T2.SubItems.Add(Buffer(1))
                            T2.SubItems.Add(Buffer(2))
                            T2.SubItems.Add(Buffer(3))
                            T2.SubItems.Add(Buffer(4))
                            T2.SubItems.Add(Buffer(5))
                            Userlist.Items.Add(T2)
                        ElseIf UBound(Buffer) = 3 Then
                            Dim T2 As New ListViewItem
                            T2.Text = Buffer(0)
                            T2.SubItems.Add(Buffer(1))
                            T2.SubItems.Add(Buffer(2))
                            T2.SubItems.Add(Buffer(3))
                            Userlist.Items.Add(T2)
                        Else
                            Userlist.Items.Add(Buffer(0)).SubItems.Add(Buffer(1))
                        End If
                    End If
                Next
            End If
        End Using
        Usercount.Text = "TOTAL USERS: " & Userlist.Items.Count
    End Sub
    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Dim S As New StringBuilder
        For Each itm As ListViewItem In Userlist.Items
            If itm.Index = Userlist.Items.Count - 1 Then
                Dim SSS As String = ""
                For Each SI As ListViewItem.ListViewSubItem In itm.SubItems
                    If SI.Text <> "" Then : SSS += SI.Text & ":" : End If
                Next
                SSS = SSS.Remove(SSS.Length - 1, 1)
                S.Append(SSS)
            Else
                Dim SSS As String = ""
                For Each SI As ListViewItem.ListViewSubItem In itm.SubItems
                    If SI.Text <> "" Then : SSS += SI.Text & ":" : End If
                Next
                SSS = SSS.Remove(SSS.Length - 1, 1)
                S.AppendLine(SSS)
            End If
        Next
        Dim SD As New SaveFileDialog
        SD.Filter = "Text Files (*.txt)|*.txt"
        SD.FilterIndex = 1
        SD.Title = "Export List"
        SD.FileName = "Accounts.txt"
        If SD.ShowDialog = DialogResult.OK Then
            Dim Sys As New System.IO.StreamWriter(SD.FileName)
            Sys.Write(S.ToString)
            Sys.Flush()
            Sys.Dispose()
            F.Logs("Password Changer list exported to " & SD.FileName, 2, frmLog.IL, frmLog.Log)
        End If
    End Sub
    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        Dim i = 0
        For Each itm As ListViewItem In Userlist.Items
            If itm.BackColor = Color.LightPink Then
                itm.Remove()
                i += 1
            End If
        Next
        Usercount.Text = "TOTAL USERS: " & Userlist.Items.Count
        Me.Refresh()
        MsgBox("Removed " & i & " invalid accounts")
    End Sub
    Dim Users As New Queue
    Sub CheckUsers()
        Try
            Dim S As String = Users.Dequeue
            Dim PLIT As String() = Split(S, ":")
            Dim U As String
            Dim P As String
            Dim I As Integer
            Dim PIP As String
            Dim PPORT As String
            Dim PU As String
            Dim PP As String
            If UBound(PLIT) = 6 Then
                U = PLIT(0)
                P = PLIT(1)
                I = PLIT(2)
                PIP = PLIT(3)
                PPORT = PLIT(4)
                PU = PLIT(5)
                PP = PLIT(6)
            ElseIf UBound(PLIT) = 4 Then
                U = PLIT(0)
                P = PLIT(1)
                I = PLIT(2)
                PIP = PLIT(3)
                PPORT = PLIT(4)
            Else
                U = PLIT(0)
                P = PLIT(1)
                I = PLIT(2)
            End If

            Dim YT As YouTube
            If PU <> Nothing Then
                YT = New YouTube(U, P, Nothing, True, PIP, PPORT, PU, PP)
            ElseIf PIP <> Nothing Then
                YT = New YouTube(U, P, Nothing, True, PIP, PPORT)
            Else
                YT = New YouTube(U, P, Nothing)
            End If

            If YT.Login() = "Yes" Then
                Dim NewPass As String = F.Spin(lstPasswords.Items(F.RandomInteger(0, lstPasswords.Items.Count - 1)).Text)
                If YT.ChangePassword(NewPass) Then
                    Userlist.Items(I).BackColor = Color.LightGreen
                    Dim LV As ListViewItem
                    LV = Userlist.Items(I)
                    LV = LV.Clone
                    LV.SubItems(1).Text = NewPass
                    lstNew.Items.Add(LV)
                Else
                    Userlist.Items(I).BackColor = Color.LightPink
                End If
            Else
                Userlist.Items(I).BackColor = Color.LightPink
            End If
            If Users.Count > 0 Then
                CheckUsers()
            Else
                btnStart.Text = "Start Changing"
                Me.Refresh()
            End If
        Catch
        End Try
    End Sub
    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        If btnStart.Text = "Start Changing" Then
            If Userlist.Items.Count > 0 Then
                If lstPasswords.Items.Count > 0 Then
                    For Each itm As ListViewItem In Userlist.Items
                        If itm.SubItems.Count = 6 Then
                            Users.Enqueue(itm.Text & ":" & itm.SubItems(1).Text & ":" & itm.Index & ":" & itm.SubItems(2).Text & ":" & itm.SubItems(3).Text & ":" & itm.SubItems(4).Text & ":" & itm.SubItems(5).Text)
                        ElseIf itm.SubItems.Count = 4 Then
                            Users.Enqueue(itm.Text & ":" & itm.SubItems(1).Text & ":" & itm.Index & ":" & itm.SubItems(2).Text & ":" & itm.SubItems(3).Text)
                        Else
                            Users.Enqueue(itm.Text & ":" & itm.SubItems(1).Text & ":" & itm.Index)
                        End If
                    Next
                Else
                    MsgBox("There must at least be one password in the password list")
                End If
            End If
            If Users.Count > 0 Then
                For i = 0 To MaxThread - 1
                    Dim T As New Threading.Thread(AddressOf CheckUsers)
                    T.Start()
                Next
                btnStart.Text = "Stop Changing"
            End If
        ElseIf btnStart.Text = "Stop Changing" Then
            Users.Clear()
            btnStart.Text = "Start Changing"
            Me.Refresh()
        End If
    End Sub
    Private Sub MenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem14.Click
        Dim S As New StringBuilder
        For Each itm As ListViewItem In lstNew.Items
            If itm.Index = lstNew.Items.Count - 1 Then
                Dim SSS As String = ""
                For Each SI As ListViewItem.ListViewSubItem In itm.SubItems
                    If SI.Text <> "" Then : SSS += SI.Text & ":" : End If
                Next
                SSS = SSS.Remove(SSS.Length - 1, 1)
                S.Append(SSS)
            Else
                Dim SSS As String = ""
                For Each SI As ListViewItem.ListViewSubItem In itm.SubItems
                    If SI.Text <> "" Then : SSS += SI.Text & ":" : End If
                Next
                SSS = SSS.Remove(SSS.Length - 1, 1)
                S.AppendLine(SSS)
            End If
        Next
        Dim SD As New SaveFileDialog
        SD.Filter = "Text Files (*.txt)|*.txt"
        SD.FilterIndex = 1
        SD.Title = "Export List"
        SD.FileName = "Accounts.txt"
        If SD.ShowDialog = DialogResult.OK Then
            Dim Sys As New System.IO.StreamWriter(SD.FileName)
            Sys.Write(S.ToString)
            Sys.Flush()
            Sys.Dispose()
            F.Logs("Account Checker list exported to " & SD.FileName, 2, frmLog.IL, frmLog.Log)
        End If
    End Sub
    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem10.Click
        If MsgBox("Are you sure you want to clear this list") = MsgBoxResult.Ok Then
            lstNew.Items.Clear()
        End If
    End Sub
    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        For Each itm As ListViewItem In lstNew.Items
            itm.Remove()
        Next
    End Sub
    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem12.Click
        Using T As New OpenFileDialog
            If T.ShowDialog = DialogResult.OK Then
                Dim Users = F.Lines(IO.File.ReadAllText(T.FileName))
                For I = 0 To Users.Length - 1
                    Dim T2 As New ListViewItem
                    T2.Text = Users(I)
                    lstPasswords.Items.Add(T2)
                Next
            End If
        End Using
    End Sub
    Private Sub MenuItem15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem15.Click
        Dim Response As String
        Response = InputBox("Please enter a password to add to the list")
        If Response <> Nothing Then
            lstPasswords.Items.Add(Response)
        End If
    End Sub
    Private Sub MenuItem16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem16.Click
        lstPasswords.Items.Clear()
    End Sub
End Class