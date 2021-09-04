Friend Module Program
    Public Sub Main()
        Dim engine As New Saga.Engine
        engine.LoadGame("NorthernLights.json")
        engine.Run()
    End Sub
End Module
