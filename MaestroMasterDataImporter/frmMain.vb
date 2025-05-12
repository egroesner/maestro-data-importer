Public Class frmMain

    Private Sub btnImportCharact_Click(sender As System.Object, e As System.EventArgs) Handles btnImportCharact.Click
        Dim ExcelApp As Object

        Try
            Dim linhasImportadas As Integer = 0
            ExcelApp = GetObject(, "Excel.Application")

            If (ExcelApp Is Nothing) Then
                Exit Sub
            End If

            ExcelApp.Visible = True

            While (ExcelApp.ActiveCell.Formula <> "")
                Dim name As String = ExcelApp.ActiveCell.Formula
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Dim language As String = ExcelApp.ActiveCell.Formula
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Dim description As String = ExcelApp.ActiveCell.Formula
                ImportarCaracteristica(name, language, description)
                ExcelApp.ActiveCell.Offset(1, -2).Range("A1").Select()
                linhasImportadas += 1
                lblStatus.Text = "Linhas Importadas: " & linhasImportadas
                Me.Refresh()
            End While
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnImportValues_Click(sender As System.Object, e As System.EventArgs) Handles btnImportValues.Click
        Dim ExcelApp As Object

        Try
            Dim linhasImportadas As Integer = 0
            ExcelApp = GetObject(, "Excel.Application")

            If (ExcelApp Is Nothing) Then
                Exit Sub
            End If

            ExcelApp.Visible = True

            While (ExcelApp.ActiveCell.Formula <> "")
                Dim name As String = ExcelApp.ActiveCell.Formula
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Dim language As String = ExcelApp.ActiveCell.Formula
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Dim code As String = ExcelApp.ActiveCell.Formula
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Dim description As String = ExcelApp.ActiveCell.Formula
                ImportarValor(name, language, code, description)
                ExcelApp.ActiveCell.Offset(1, -3).Range("A1").Select()
                linhasImportadas += 1
                lblStatus.Text = "Linhas Importadas: " & linhasImportadas
                Me.Refresh()
            End While
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ImportarCaracteristica(ByVal name As String, ByVal language As String, ByVal description As String)
        Dim servico As New IMaestroMasterDataManager.MasterDataManagerService
        Dim desc As New IMaestroMasterDataManager.mergeCharacteristicDescription
        desc.characteristicName = name
        desc.language = language
        desc.description = description

        Dim response As IMaestroMasterDataManager.mergeCharacteristicDescriptionResponse
        servico.Url = txtURL.Text.Trim()

        servico.User = New IMaestroMasterDataManager.string()
        Dim userarr = New System.Collections.ArrayList
        userarr.Add(System.Environment.GetEnvironmentVariable("USERNAME").ToLower().Trim())
        servico.User.Text = userarr.ToArray(GetType(String))

        response = servico.mergeCharacteristicDescription(desc)
        If response.MasterDataManager = False Then
            Throw New System.Exception("Erro ao importar dado.")
        End If
    End Sub

    Private Sub ImportarValor(ByVal name As String, ByVal language As String, ByVal code As String, ByVal description As String)
        Dim servico As New IMaestroMasterDataManager.MasterDataManagerService
        Dim desc As New IMaestroMasterDataManager.mergeValueDescription
        desc.characteristicName = name
        desc.language = language
        desc.characteristicValue = code
        desc.description = description

        Dim response As IMaestroMasterDataManager.mergeValueDescriptionResponse
        servico.Url = txtURL.Text.Trim()

        servico.User = New IMaestroMasterDataManager.string()
        Dim userarr = New System.Collections.ArrayList
        userarr.Add(System.Environment.GetEnvironmentVariable("USERNAME").ToLower().Trim())
        servico.User.Text = userarr.ToArray(GetType(String))

        response = servico.mergeValueDescription(desc)
        If response.MasterDataManager = False Then
            Throw New System.Exception("Erro ao importar dado.")
        End If
    End Sub

    Private Sub btnSair_Click(sender As System.Object, e As System.EventArgs) Handles btnSair.Click
        Me.Close()
    End Sub

    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        txtURL.Text = System.Configuration.ConfigurationManager.AppSettings.Get("MaestroMasterDataURL")
        txtRulesURL.Text = System.Configuration.ConfigurationManager.AppSettings.Get("RuleManagerServiceURL")
    End Sub

    Private Sub ImportarRegras(ByVal template As Template)
        Dim service As New IRuleManagerServiceSoapService.RuleManagerServiceSoapService
        Dim request As New IRuleManagerServiceSoapService.ruleManagerRequest

        request.ruleTemplateId = template.Id

        Dim ruleComponent As IRuleManagerServiceSoapService.ruleComponent

        Dim ruleArrayList As New System.Collections.ArrayList()
        For Each excelRule As Rule In template.Rules
            Dim ruleComponentArrayList As New System.Collections.ArrayList()
            For Each excelCharact In excelRule.Characteristics
                ruleComponent = New IRuleManagerServiceSoapService.ruleComponent()
                ruleComponent.characteristicName = excelCharact.Name

                Dim ruleComponentValueArrayList As New System.Collections.ArrayList()
                For Each excelValue In excelCharact.Values
                    ruleComponentValueArrayList.Add(excelValue)
                Next
                ruleComponent.value = ruleComponentValueArrayList.ToArray(GetType(String))
                ruleComponentArrayList.Add(ruleComponent)
            Next
            Dim ruleComponentArray As IRuleManagerServiceSoapService.ruleComponent() = DirectCast(ruleComponentArrayList.ToArray(GetType(IRuleManagerServiceSoapService.ruleComponent)), IRuleManagerServiceSoapService.ruleComponent())
            ruleArrayList.Add(ruleComponentArray)
        Next

        Dim ruleArray As IRuleManagerServiceSoapService.ruleComponent()() = DirectCast(ruleArrayList.ToArray(GetType(IRuleManagerServiceSoapService.ruleComponent())), IRuleManagerServiceSoapService.ruleComponent()())
        request.ruleList = ruleArray

        service.Url = txtRulesURL.Text

        service.Username = New IRuleManagerServiceSoapService.string()
        Dim userarr = New System.Collections.ArrayList
        userarr.Add(System.Environment.GetEnvironmentVariable("USERNAME").ToLower().Trim())
        service.Username.Text = userarr.ToArray(GetType(String))

        If service.saveRules(request) <> True Then
            Throw New System.Exception("Erro ao importar regras.")
        End If
    End Sub

    Private Sub btnImportRules_Click(sender As System.Object, e As System.EventArgs) Handles btnImportRules.Click
        Dim ExcelApp As Object
        Dim nextAddress = "$B$2"

        Try
            Dim linhasImportadas As Integer = 0
            ExcelApp = GetObject(, "Excel.Application")

            If (ExcelApp Is Nothing) Then
                Exit Sub
            End If

            ExcelApp.Visible = True

            Dim cell As String = ExcelApp.ActiveCell.Address
            If (cell.IndexOf("$B") = 0) And (cell <> "$B$1") Then
                nextAddress = cell
            End If

            Dim template As New Template()

            'Carregar o Id
            ExcelApp.Range("A1").Select()
            Dim aux As String = Strings.Trim(ExcelApp.ActiveCell.Formula)
            template.Id = Long.Parse(aux)

            ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()

            'Carregar o cabeçario de Caracteristicas
            While (ExcelApp.ActiveCell.Formula <> "")
                template.CharacteristicColumns.Add(Strings.Trim(ExcelApp.ActiveCell.Formula))
                ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
            End While

            ExcelApp.Range(nextAddress).Select()

            'Carregar os valores de Caracteristicas
            Dim line As Integer = 0
            While (ExcelApp.ActiveCell.Formula <> "")
                Dim rule As New Rule()
                Dim characteristic As Characteristic

                For i = 0 To template.CharacteristicColumns.Count - 1
                    characteristic = New Characteristic()
                    characteristic.Name = template.CharacteristicColumns(i)
                    aux = ""
                    If Not String.IsNullOrEmpty(ExcelApp.ActiveCell.Formula) Then
                        aux = ExcelApp.ActiveCell.Formula
                        If aux.IndexOf("FORMULA=") = 0 Then
                            characteristic.Values.Add(Strings.Trim(aux.Substring(8, aux.Length - 8)))
                        Else
                            If (aux.IndexOf(";") >= 0) Then
                                For Each subvalue As String In aux.Split(";")
                                    If Not String.IsNullOrEmpty(subvalue) Then
                                        characteristic.Values.Add(Strings.Trim(subvalue))
                                    End If
                                Next
                            Else
                                characteristic.Values.Add(Strings.Trim(aux))
                            End If
                        End If
                    End If
                    rule.Characteristics.Add(characteristic)
                    ExcelApp.ActiveCell.Offset(0, 1).Range("A1").Select()
                Next
                template.Rules.Add(rule)
                ExcelApp.ActiveCell.Offset(1, -(template.CharacteristicColumns.Count)).Range("A1").Select()
                linhasImportadas += 1
                lblStatus.Text = "Linhas Lidas: " & linhasImportadas
                Me.Refresh()

                line += 1

                If line >= 50 Then
                    lblStatus.Text = "Importando Regras."
                    Me.Refresh()
                    ImportarRegras(template)
                    lblStatus.Text = "Regras Importadas."
                    Me.Refresh()

                    template.Rules = New System.Collections.Generic.List(Of Rule)()
                    nextAddress = ExcelApp.ActiveCell.Address
                    line = 0
                End If
            End While

            If template.Rules.Count > 0 Then
                lblStatus.Text = "Importando Regras."
                Me.Refresh()
                ImportarRegras(template)
                lblStatus.Text = "Regras Importadas."
                Me.Refresh()
            End If

        Catch ex As Exception
            Try
                ExcelApp.Range(nextAddress).Select()
            Catch innerex As Exception
            End Try
            System.Windows.Forms.MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Windows.Forms.MessageBox.Show("A nova importação deve começar em " + nextAddress.Replace("$", ""), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class
