Option Strict On

Namespace Saga
    Public Class Engine
        ' Various flags to control the game's state.
        Private GameLoaded As Boolean = False
        Private AskingForName As Boolean = True
        Private Running As Boolean = False
        Private DescriptionNeeded As Boolean = False

        Private Game As New Game
        Private Player As New Player

        Public Sub LoadGame(gameFilename As String)
            Dim majorVersion As String = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Major.ToString()
            Dim minorVersion As String = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Minor.ToString()
            Console.WriteLine($"You are running the Saga retro engine for Single-User Dungeons, version {majorVersion}.{minorVersion}.")
            Console.WriteLine($"Loading saga ""{gameFilename}""...")

            ' Populate the Game object from the provided JSON file.
            GameLoaded = Game.LoadFromJson(gameFilename)
            If GameLoaded Then
                Console.Title = $"Saga v{majorVersion}.{minorVersion} - {Game.Name}"
            End If
        End Sub

        Public Sub Run()
            If Not GameLoaded Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("No game has been loaded!")
                Console.ResetColor()
                Return
            End If

            Console.WriteLine()
            Console.WriteLine($"Welcome to ""{Game.Name}""! What is your name?")
            Do While AskingForName
                ' Prompt for the player's name.
                Console.Write("> ")
                Dim input = Console.ReadLine()
                HandleNameEntry(input.Trim())
            Loop

            ' Main game loop.
            DescriptionNeeded = True
            Do While Running
                Console.WriteLine()

                ' Try to ensure our current room exists.
                CheckCurrentRoom()

                ' Print the current room name always, but the description only if needed.
                If DescriptionNeeded Then
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.WriteLine(Game.GetRoom(Player.CurrentRoom).Name)
                    Console.ResetColor()
                    Game.GetRoom(Player.CurrentRoom).WriteDescription()
                    DescriptionNeeded = False
                Else
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.WriteLine(Game.GetRoom(Player.CurrentRoom).Name)
                    Console.ResetColor()
                End If

                ' Prompt for the player's input.
                Console.Write("> ")
                Dim input = Console.ReadLine()
                HandleInput(input.Trim())
            Loop

            Player.Save()
        End Sub

        Private Sub HandleNameEntry(input As String)
            ' If we receive Ctrl+C, for example, exit.
            If input Is Nothing Or input = "exit" Or input = "quit" Then
                AskingForName = False
                Return
            End If

            If input = "" Then
                Return
            End If

            If System.IO.File.Exists(input + ".json") Then
                ' If a file exists by this name, try loading it.
                If Player.Load(input) Then
                    Console.WriteLine($"Welcome back, {Player.Name}!")
                    AskingForName = False
                    Running = True
                End If
            Else
                ' Otherwise, start a new game.
                Player.Name = input
                Player.CurrentRoom = Game.StartingRoom
                Player.Save()
                Console.WriteLine($"Pleased to meet you, {Player.Name}!")
                AskingForName = False
                Running = True
            End If
        End Sub

        Private Sub CheckCurrentRoom()
            If Game.GetRoom(Player.CurrentRoom) Is Nothing Then
                ' Uh oh. How did we get here?
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"Room ""{Player.CurrentRoom}"" wasn't found. How did you get here?")
                Console.ResetColor()
                ' TODO: Make sure a Limbo room exists too.
                Player.CurrentRoom = "Limbo"
                DescriptionNeeded = True
            End If
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

            input = input.ToLower()

            input = LookupShorthand(input)

            Select Case input
                Case "exit", "quit"
                    Console.WriteLine("Good-bye!")
                    Running = False
                    inputUnderstood = True
                Case "help", "?"
                    inputUnderstood = DoHelp()
                Case "look"
                    DescriptionNeeded = True
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

        Private Shared Function DoHelp() As Boolean
            Console.WriteLine("Type ""exit"" or ""quit"" to finish playing.")
            Console.WriteLine("     ""help"" or ""?"" to see these instructions.")
            Console.WriteLine("     ""look"" to look around at your surroundings.")
            Console.WriteLine("     ""north"", ""n"", ""south"", etc to move around the environment.")
            Return True
        End Function

        Private Function TryExit(input As String) As Boolean
            If Game.GetRoom(Player.CurrentRoom).HasExit(input) Then
                Dim newRoom As String = Game.GetRoom(Player.CurrentRoom).GetExit(input).Room
                If Game.GetRoom(newRoom) IsNot Nothing Then
                    Player.CurrentRoom = newRoom
                    DescriptionNeeded = True
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
