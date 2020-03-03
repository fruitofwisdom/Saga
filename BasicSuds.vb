Option Strict On

Namespace BasicSuds
    Public Class BasicSudsEngine
        Private Running As Boolean

        Public Sub New()
            Running = False
        End Sub

        Public Sub Run(Game As String)
            ' TODO: Actually load game content.
            Console.WriteLine("Running Single User Dungeon " & Game & "...")
            Running = True

            ' Main game loop.
            Do While Running
                Console.Write("> ")
                Dim Input = Console.ReadLine()

                ' Handle player input.
                If Input Is Nothing Or Input.ToLower() = "exit" Or Input.ToLower() = "quit" Then
                    Running = False
                ElseIf Input.ToLower() = "help" Or Input.ToLower() = "?" Then
                    Console.WriteLine("BasicSucs Basic Help")
                    Console.WriteLine("Type ""exit"" or ""quit"" to finish playing.")
                    Console.WriteLine("     ""help"" or ""?"" to see these instructions.")
                End If
            Loop
        End Sub
    End Class
End Namespace
