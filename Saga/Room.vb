Option Strict On

Namespace Saga
    Friend Structure RoomExit
        Public Property Direction As String
        Public Property Room As String
    End Structure

    Friend Class Room
        Public Property Name As String
        Public Property Description As String
        Public Property Exits As List(Of RoomExit)

        Public Function HasExit(direction As String) As Boolean
            Dim toReturn As Boolean = False

            For Each roomExit In Exits
                If roomExit.Direction = direction Then
                    toReturn = True
                End If
            Next

            Return toReturn
        End Function

        Public Function GetExit(direction As String) As RoomExit
            Dim toReturn As RoomExit = Nothing

            For Each roomExit In Exits
                If roomExit.Direction = direction Then
                    toReturn = roomExit
                End If
            Next

            Return toReturn
        End Function

        ' Write the description of a Room to the console with automatic line breaks.
        Public Sub WriteDescription()
            Dim line As String = ""
            Dim width As Integer = 0
            Dim words As String() = Description.Split()

            For i As Integer = 0 To words.Length - 1
                line += words(i)
                width += words(i).Length

                If i = words.Length - 1 Then
                    Console.WriteLine(line)
                Else
                    If width + words(i + 1).Length >= Console.WindowWidth - 1 Then
                        Console.WriteLine(line)
                        line = ""
                        width = 0
                    Else
                        line += " "
                        width += 1
                    End If
                End If
            Next i
        End Sub
    End Class
End Namespace
