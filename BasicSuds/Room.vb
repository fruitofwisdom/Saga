Option Strict On

Namespace BasicSuds
    Friend Class Room
        Public Property Name As String
        Public Property Description As String
        Public Property Exits As Dictionary(Of String, String)

        Public Shared Function LoadFromXml(gameXml As XElement) As Dictionary(Of String, Room)
            Dim rooms As New Dictionary(Of String, Room)
            Dim roomsXml As IEnumerable(Of XElement) = From roomXml In gameXml...<room>
            For Each roomXml In roomsXml
                If roomXml.@name Is Nothing Or roomXml.Element("description") Is Nothing Then
                    Console.WriteLine("Found an invalid room, ignoring!")
                Else
                    ' All child elements of <exit> are links to other rooms.
                    Dim exits As New Dictionary(Of String, String)
                    If roomXml.Element("exits") IsNot Nothing Then
                        For Each roomExit In roomXml.Elements("exits").Elements()
                            If roomExit.Value = "" Then
                                Console.WriteLine($"Found an invalid exit for room ""{roomXml.@name}"", ignoring!")
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
    End Class
End Namespace
