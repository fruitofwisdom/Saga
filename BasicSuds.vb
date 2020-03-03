Option Strict On

Namespace BasicSuds
    Public Class Engine
        Private Structure Room
            Public Property Name As String
            Public Property Description As String
        End Structure

        Private GameLoaded As Boolean = False
        Private Running As Boolean = False

        Private ReadOnly Rooms As New Dictionary(Of String, Room)
        Private CurrentRoom As String

        Public Sub LoadGame(gameFilename As String)
            Console.WriteLine($"Loading Single User Dungeon ""{gameFilename}""...")
            Try
                Dim gameXml As XElement = XElement.Load(gameFilename)

                ' Load room elements into our Rooms dictionary.
                Dim roomsXml As IEnumerable(Of XElement) = From roomXml In gameXml...<room>
                For Each roomXml In roomsXml
                    If IsNothing(roomXml.@name) Or IsNothing(roomXml.Element("description")) Then
                        Console.WriteLine("Found an invalid room, ignoring!")
                    Else
                        Dim newRoom As New Room With {
                            .Name = roomXml.@name,
                            .Description = roomXml.Element("description").Value
                            }
                        Rooms.Add(newRoom.Name, newRoom)
                    End If
                Next
                CurrentRoom = gameXml.@startingRoom

                GameLoaded = True
                Console.WriteLine($"Ready to play {gameXml.@name}!")
            Catch exception As System.IO.FileNotFoundException
                Console.WriteLine("Game not found!")
            Catch exception As System.Xml.XmlException
                Console.WriteLine("Malformed XML file!")
            End Try
        End Sub

        Public Sub Run()
            If Not GameLoaded Then
                Console.WriteLine("No game has been loaded!")
                Return
            End If
            Running = True

            ' Main game loop.
            Do While Running
                Console.WriteLine()
                Console.WriteLine(Rooms.Item(CurrentRoom).Name)
                Console.WriteLine(Rooms.Item(CurrentRoom).Description)
                Console.Write("> ")
                Dim input = Console.ReadLine()
                HandleInput(input)
            Loop
        End Sub

        Private Sub HandleInput(input As String)
            Dim inputUnderstood As Boolean = False

            ' If we receive Ctrl+C, for example, exit.
            If input Is Nothing Then
                input = "exit"
            End If

            input = input.Trim().ToLower()

            Select Case input
                Case "exit", "quit"
                    Running = False
                    inputUnderstood = True
                Case "help", "?"
                    Console.WriteLine("BasicSuds Basic Help")
                    Console.WriteLine("Type ""exit"" or ""quit"" to finish playing.")
                    Console.WriteLine("     ""help"" or ""?"" to see these instructions.")
                    inputUnderstood = True
                Case ""
                    inputUnderstood = True
            End Select

            If Not inputUnderstood Then
                Console.WriteLine("Huh?")
            End If
        End Sub
    End Class
End Namespace
