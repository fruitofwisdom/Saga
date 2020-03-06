Option Strict On

Namespace BasicSuds
    Public Class Engine
        Private GameLoaded As Boolean = False
        Private Running As Boolean = False

        Private Rooms As New Dictionary(Of String, Room)
        Private CurrentRoom As String

        Public Sub LoadGame(gameFilename As String)
            Console.WriteLine($"Loading Single User Dungeon ""{gameFilename}""...")
            Try
                Dim gameXml As XElement = XElement.Load(gameFilename)

                ' Load room elements into our Rooms dictionary.
                Rooms = Room.LoadFromXml(gameXml)
                CurrentRoom = gameXml.@startingRoom

                GameLoaded = True
                Console.WriteLine($"Ready to play ""{gameXml.@name}""!")
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

        ' Look up some of the most common commands' abbreviated shorthand.
        Private Function LookupShorthand(input As String) As String
            Dim shorthand = New Dictionary(Of String, String) From {
                {"n", "north"},
                {"s", "south"},
                {"e", "east"},
                {"w", "west"},
                {"ne", "northeast"},
                {"nw", "northwest"},
                {"se", "southeast"},
                {"sw", "southwest"},
                {"u", "up"},
                {"d", "down"}
                }
            If shorthand.ContainsKey(input) Then
                Return shorthand.Item(input)
            Else
                Return input
            End If
        End Function

        Private Sub HandleInput(input As String)
            Dim inputUnderstood As Boolean = False

            ' If we receive Ctrl+C, for example, exit.
            If input Is Nothing Then
                input = "exit"
            End If

            input = input.Trim().ToLower()

            input = LookupShorthand(input)

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
                Case Else
                    ' Room exits can be non-standard commands. Handle checking those here.
                    If Rooms.Item(CurrentRoom).Exits.ContainsKey(input) Then
                        Dim newRoom As String = Rooms.Item(CurrentRoom).Exits.Item(input)
                        If Rooms.ContainsKey(newRoom) Then
                            CurrentRoom = newRoom
                        Else
                            Console.WriteLine($"Room ""{newRoom}"" wasn't found!")
                        End If
                    Else
                        Console.WriteLine($"You can't ""{input}"" here.")
                    End If
                    inputUnderstood = True
            End Select

            ' TODO: This is no longer necessary, I think...
            If Not inputUnderstood Then
                Console.WriteLine("Huh?")
            End If
        End Sub
    End Class
End Namespace
