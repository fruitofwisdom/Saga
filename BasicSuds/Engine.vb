Option Strict On

Namespace BasicSuds
    Public Class Engine
        Private GameLoaded As Boolean = False
        Private Running As Boolean = False
        Private NeedLook As Boolean = False
        Private Version As String = "alpha.210624"

        Private Rooms As New Dictionary(Of String, Room)
        Private CurrentRoom As String

        Public Sub LoadGame(gameFilename As String)
            Console.WriteLine($"Welcome to the BasicSuds retro engine for Single User Dungeons, version {Version}!")
            Console.WriteLine($"Loading Single User Dungeon ""{gameFilename}""...")
            Try
                Dim gameXml As XElement = XElement.Load(gameFilename)

                ' Load room elements into our Rooms dictionary.
                Rooms = Room.LoadFromXml(gameXml)
                CurrentRoom = gameXml.@startingRoom

                GameLoaded = True
                Console.Title = $"BasicSuds {Version} - {gameXml.@name}"
                Console.WriteLine($"{Rooms.Count} rooms loaded. Ready to play ""{gameXml.@name}""!")
            Catch exception As System.IO.FileNotFoundException
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Game not found!")
                Console.ResetColor()
            Catch exception As System.Xml.XmlException
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Malformed XML file!")
                Console.ResetColor()
            End Try
        End Sub

        Public Sub Run()
            If Not GameLoaded Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("No game has been loaded!")
                Console.ResetColor()
                Return
            End If

            Running = True
            NeedLook = True

            ' Main game loop.
            Do While Running
                Console.WriteLine()
                If NeedLook Then
                    Console.WriteLine(Rooms.Item(CurrentRoom).Name)
                    Rooms.Item(CurrentRoom).WriteDescription()
                    NeedLook = False
                End If
                Console.Write("> ")
                Dim input = Console.ReadLine()
                HandleInput(input)
            Loop
        End Sub

        ' Look up some of the most common commands' abbreviated shorthand.
        Private Function LookupShorthand(input As String) As String
            Dim shorthand = New Dictionary(Of String, String) From {
                {"n", "north"},
                {"ne", "northeast"},
                {"e", "east"},
                {"se", "southeast"},
                {"s", "south"},
                {"sw", "southwest"},
                {"w", "west"},
                {"nw", "northwest"},
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
                    Console.WriteLine("Good-bye!")
                    Running = False
                    inputUnderstood = True
                Case "help", "?"
                    inputUnderstood = DoHelp()
                Case "look"
                    NeedLook = True
                    inputUnderstood = True
                Case ""
                    inputUnderstood = True
                Case Else
                    ' Room exits can be non-standard commands. Handle checking those here.
                    inputUnderstood = TryExit(input)
            End Select

            ' TODO: This is no longer necessary, I think...
            If Not inputUnderstood Then
                Console.WriteLine("Huh?")
            End If
        End Sub

        Private Function DoHelp() As Boolean
            Console.WriteLine("Type ""exit"" or ""quit"" to finish playing.")
            Console.WriteLine("     ""help"" or ""?"" to see these instructions.")
            Console.WriteLine("     ""look"" to look around at your surroundings.")
            Console.WriteLine("     ""north"", ""n"", ""south"", etc to move around the environment.")
            Return True
        End Function

        Private Function TryExit(input As String) As Boolean
            If Rooms.Item(CurrentRoom).Exits.ContainsKey(input) Then
                Dim newRoom As String = Rooms.Item(CurrentRoom).Exits.Item(input)
                If Rooms.ContainsKey(newRoom) Then
                    CurrentRoom = newRoom
                    NeedLook = True
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine($"Room ""{newRoom}"" wasn't found!")
                    Console.ResetColor()
                End If
            Else
                Console.WriteLine($"You can't ""{input}"" here.")
            End If

            Return True
        End Function
    End Class
End Namespace
