Friend Module Program
    Public Sub Main()
        Dim engine As New BasicSuds.Engine
        engine.LoadGame("NorthernLights.xml")
        engine.Run()
    End Sub
End Module
