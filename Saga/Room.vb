Option Strict On

Namespace Saga
    Friend Class Room
        Public Property Name As String
        Public Property Description As String
        Public Property Exits As Dictionary(Of String, String)

        ' Parse some XML into a dictionary of rooms by room name.
        Public Shared Function LoadFromXml(gameXml As XElement) As Dictionary(Of String, Room)
            Dim rooms As New Dictionary(Of String, Room)
            Dim roomsXml As IEnumerable(Of XElement) = From roomXml In gameXml...<room>

            For Each roomXml In roomsXml
                If roomXml.@name Is Nothing Or roomXml.Element("description") Is Nothing Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Found an invalid room, ignoring!")
                    Console.ResetColor()
                Else
                    ' All child elements of <exit> are links to other rooms.
                    Dim exits As New Dictionary(Of String, String)
                    If roomXml.Element("exits") IsNot Nothing Then
                        For Each roomExit In roomXml.Elements("exits").Elements()
                            If roomExit.Value = "" Then
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine($"Found an invalid exit for room ""{roomXml.@name}"", ignoring!")
                                Console.ResetColor()
                            Else
                                exits.Add(roomExit.Name.ToString().Trim(), roomExit.Value.Trim())
                            End If
                        Next
                    End If

                    Dim newRoom As New Room With {
                        .Name = roomXml.@name.Trim(),
                        .Description = roomXml.Element("description").Value.Trim(),
                        .Exits = exits
                        }
                    rooms.Add(newRoom.Name, newRoom)
                End If
            Next

            Return rooms
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
