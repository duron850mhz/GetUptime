Imports System.IO
Imports System.Threading
Module mdlMain
    Dim UPTIME_FILE As String = "last_uptime"
    Dim LOG_FILE As String = "uptime.log"
    Dim LG_MyPath As String = My.Application.Info.DirectoryPath

    Sub Main()
        Do
            Call I_Tick()
            Thread.Sleep(60000)
        Loop
    End Sub

    Private Function I_GetSystemUpTime() As Single
        Dim upTime As New PerformanceCounter("System", "System Up Time")

        upTime.NextValue() ' 戻り値は0となる
        Return upTime.NextValue() ' 2度呼び出す必要あり
    End Function

    Private Sub I_Tick()
        '前回分取得
        Dim LAST_UPTIME As Single = 0
        Dim strUptimeFile As String = Path.Combine(LG_MyPath, UPTIME_FILE)
        Try
            Using sr As New StreamReader(strUptimeFile)
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
                Dim dtLastTime As Date = File.GetLastWriteTime(strUptimeFile)
                If (Date.Now < "2019/11/01 00:00:00") Then
                    dtLastTime = Date.Now   'バグってたけど途中で変えるとアレなので11/1からよ
                End If
                sw.WriteLine(dtLastTime.ToString("yyyy/MM/dd HH:mm:ss") & "," & LAST_UPTIME.ToString)
                sw.Close()
            End Using
        End If

        '更新
        Using sw As New StreamWriter(Path.Combine(LG_MyPath, UPTIME_FILE))
            sw.WriteLine(NOW_UPTIME.ToString)
            sw.Close()
        End Using
    End Sub
End Module
