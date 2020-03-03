Friend Module Program
    Public Sub Main()
        Console.WriteLine("Welcome to the BasicSuds retro engine for Single User Dungeons!")
        Dim Engine As New BasicSuds.BasicSudsEngine
        Engine.Run("DemoGame.xml")
    End Sub
End Module
