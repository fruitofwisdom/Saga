Friend Module Program
    Public Sub Main()
        Dim engine As New Saga.Engine
        engine.LoadGame("NorthernLights.xml")
        engine.Run()
    End Sub
End Module
