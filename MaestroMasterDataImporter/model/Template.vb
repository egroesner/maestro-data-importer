Public Class Template
    Private _id As Long
    Private _CharacteristicColumns As New System.Collections.Generic.List(Of String)
    Private _rules As New System.Collections.Generic.List(Of Rule)

    Public Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property

    Public Property CharacteristicColumns() As System.Collections.Generic.List(Of String)
        Get
            Return _CharacteristicColumns
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of String))
            _CharacteristicColumns = value
        End Set
    End Property

    Public Property Rules() As System.Collections.Generic.List(Of Rule)
        Get
            Return _rules
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of Rule))
            _rules = value
        End Set
    End Property

End Class
