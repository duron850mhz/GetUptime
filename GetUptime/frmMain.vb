Imports System.IO
Public Class frmMain

    Dim UPTIME_FILE As String = "last_uptime"
    Dim LOG_FILE As String = "uptime.log"
    Dim LG_MyPath As String = My.Application.Info.DirectoryPath

    Private Function I_GetSystemUpTime() As Single
        Dim upTime As New PerformanceCounter("System", "System Up Time")

        upTime.NextValue() ' 戻り値は0となる
        Return upTime.NextValue() ' 2度呼び出す必要あり
    End Function

    Private Sub I_Tick()
        '前回分取得
        Dim LAST_UPTIME As Single
        Try
            Using sr As New StreamReader(Path.Combine(LG_MyPath, UPTIME_FILE))
                LAST_UPTIME = CSng(sr.ReadLine)
                sr.Close()
            End Using
        Catch ex As Exception
        End Try

        '今回分取得
        Dim NOW_UPTIME As Single = I_GetSystemUpTime()

        'ﾋｶｸ
        If (NOW_UPTIME < LAST_UPTIME) Then
            'ログ保存
            Using sw As New StreamWriter(Path.Combine(LG_MyPath, LOG_FILE), True)
                sw.WriteLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss") & "," & LAST_UPTIME.ToString)
                sw.Close()
            End Using
        End If

        '更新
        Using sw As New StreamWriter(Path.Combine(LG_MyPath, UPTIME_FILE))
            sw.WriteLine(NOW_UPTIME.ToString)
            sw.Close()
        End Using
        lblMsg.Text = "最終更新：" & Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Call I_Tick()
    End Sub
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblMsg.Text = ""
        Timer1.Interval = 60000
        Call I_Tick()
    End Sub
End Class