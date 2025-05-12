Public Class Characteristic
    Private _name As String
    Private _values As New System.Collections.Generic.List(Of String)

    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property Values() As System.Collections.Generic.List(Of String)
        Get
            Return _values
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of String))
            _values = value
        End Set
    End Property
End Class
