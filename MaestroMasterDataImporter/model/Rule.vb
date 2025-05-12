Public Class Rule
    Private _characteristics As New System.Collections.Generic.List(Of Characteristic)

    Public Property Characteristics() As System.Collections.Generic.List(Of Characteristic)
        Get
            Return _characteristics
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of Characteristic))
            _characteristics = value
        End Set
    End Property
End Class
