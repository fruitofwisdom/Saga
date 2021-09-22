Option Strict On

Imports Newtonsoft.Json

Namespace Saga
    Friend Class Player
        Public Name As String
        Public CurrentRoom As String

        Public Function Load(name As String) As Boolean
            Try
                Dim jsonContent = System.IO.File.ReadAllText(name + ".json")
                JsonConvert.PopulateObject(jsonContent, Me)
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"Failed to load: {ex.Message}")
                Console.ResetColor()
            End Try

            Return CurrentRoom IsNot Nothing
        End Function

        Public Sub Save()
            Try
                Dim jsonContent = JsonConvert.SerializeObject(Me)
                System.IO.File.WriteAllText(Name + ".json", jsonContent)
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"Failed to save: {ex.Message}")
                Console.ResetColor()
            End Try
        End Sub
    End Class
End Namespace
