Option Strict On

Imports Newtonsoft.Json

Namespace Saga
    Friend Class Game
        Public Name As String
        Public StartingRoom As String
        Public Rooms As List(Of Room)

        ' Populate ourselves from a provided JSON file.
        Public Function LoadFromJson(gameFilename As String) As Boolean
            Try
                Dim jsonContent = System.IO.File.ReadAllText(gameFilename)
                JsonConvert.PopulateObject(jsonContent, Me)
            Catch exception As System.IO.FileNotFoundException
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Game not found!")
                Console.ResetColor()
            Catch exception As JsonException
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Malformed JSON file!")
                Console.ResetColor()
            End Try

            Return Rooms.Count > 0 And GetRoom(StartingRoom) IsNot Nothing
        End Function

        Public Function GetRoom(roomName As String) As Room
            Dim toReturn As Room = Nothing

            For Each room In Rooms
                If room.Name = roomName Then
                    toReturn = room
                End If
            Next

            Return toReturn
        End Function

        Public Function GetRoomCount() As Integer
            Return Rooms.Count
        End Function
    End Class
End Namespace
