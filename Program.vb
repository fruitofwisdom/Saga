Friend Module Program
    Public Sub Main()
        Console.WriteLine("Welcome to the BasicSuds retro engine for Single User Dungeons!")
        Dim engine As New BasicSuds.Engine
        engine.LoadGame("DemoGame.xml")
        engine.Run()
    End Sub
End Module
