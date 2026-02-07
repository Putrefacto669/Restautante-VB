Imports System.Media
Public Class PrincipalForm
    Dim subtotal As Decimal = 0
    Private Sub PrincipalForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            My.Computer.Audio.Play(Application.StartupPath & "\login.wav",
                                    AudioPlayMode.Background)
        Catch ex As Exception
            MessageBox.Show("Error al reproducir audio: " & ex.Message)
        End Try
        ' Fecha actual
        DtpFecha.Value = Date.Now

        ' Llenar meseros
        cmbMesero.Items.Add("Chester Cheetos")
        cmbMesero.Items.Add("Gustavo")

        ' Llenar mesas
        For i As Integer = 1 To 10
            cmbMesa.Items.Add(i)
        Next

        ' Llenar platos
        lstPlatosDisponibles.Items.Add("Pizza - 200")
        lstPlatosDisponibles.Items.Add("Ensalada - 100")
        lstPlatosDisponibles.Items.Add("Sopa - 120")
        lstPlatosDisponibles.Items.Add("Jugo Natural - 80")
        lstPlatosDisponibles.Items.Add("Hamburguesa - 150")

        ' Inicializar totales
        lblTotalSinIVA.Text = "0.00"
        lblTotalConIVA.Text = "0.00"

        ' Tarjeta deshabilitada al inicio
        txtNumeroTarjeta.Enabled = False
    End Sub

    Private Sub cmbMesero_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMesero.SelectedIndexChanged
        ' Si no hay nada seleccionado, salir y limpiar imagen
        If cmbMesero.SelectedItem Is Nothing Then
            PicMesero.Image = Nothing
            Exit Sub
        End If

        Select Case cmbMesero.SelectedItem.ToString()
            Case "Chester Cheetos"
                PicMesero.Image = Image.FromFile(Application.StartupPath & "\mesero1.png")
            Case "Gustavo"
                PicMesero.Image = Image.FromFile(Application.StartupPath & "\mesero2.jpg")
        End Select
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If lstPlatosDisponibles.SelectedItem Is Nothing Then
            MessageBox.Show("Seleccione un plato.")
            Exit Sub
        End If

        Dim item As String = lstPlatosDisponibles.SelectedItem.ToString()
        lstPedido.Items.Add(item)

        ' Obtener precio
        Dim partes() As String = item.Split("-"c)
        Dim precio As Decimal = Convert.ToDecimal(partes(1).Trim())

        subtotal += precio
        ActualizarTotales()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If lstPedido.SelectedItem Is Nothing Then
            MessageBox.Show("Seleccione un plato para quitar.")
            Exit Sub
        End If

        Dim item As String = lstPedido.SelectedItem.ToString()

        ' Obtener precio
        Dim partes() As String = item.Split("-"c)
        Dim precio As Decimal = Convert.ToDecimal(partes(1).Trim())

        subtotal -= precio
        lstPedido.Items.Remove(item)

        ActualizarTotales()
    End Sub
    Private Sub ActualizarTotales()
        lblTotalSinIVA.Text = subtotal.ToString("0.00")
        Dim totalConIVA As Decimal = subtotal * 1.15D
        lblTotalConIVA.Text = totalConIVA.ToString("0.00")
    End Sub

    Private Sub rbEfectivo_CheckedChanged(sender As Object, e As EventArgs) Handles rbEfectivo.CheckedChanged
        If rbEfectivo.Checked Then
            txtNumeroTarjeta.Enabled = False
            txtNumeroTarjeta.Text = ""
        End If
    End Sub

    Private Sub rbTarjeta_CheckedChanged(sender As Object, e As EventArgs) Handles rbTarjeta.CheckedChanged
        If rbTarjeta.Checked Then
            txtNumeroTarjeta.Enabled = True
        End If
    End Sub

    Private Sub txtNumeroTarjeta_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNumeroTarjeta.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnRealizarPedido_Click(sender As Object, e As EventArgs) Handles btnRealizarPedido.Click
        If txtCliente.Text.Trim() = "" Then
            MessageBox.Show("Ingrese el nombre del cliente.")
            Exit Sub
        End If

        If cmbMesero.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione un mesero.")
            Exit Sub
        End If

        If cmbMesa.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione una mesa.")
            Exit Sub
        End If

        If lstPedido.Items.Count = 0 Then
            MessageBox.Show("Agregue al menos un plato al pedido.")
            Exit Sub
        End If

        Dim metodoPago As String = ""
        If rbEfectivo.Checked Then
            metodoPago = "Efectivo"
        ElseIf rbTarjeta.Checked Then
            If txtNumeroTarjeta.Text.Length <> 16 Then
                MessageBox.Show("La tarjeta debe tener 16 dígitos.")
                Exit Sub
            End If
            metodoPago = "Tarjeta **** **** **** " & txtNumeroTarjeta.Text.Substring(12)
        Else
            MessageBox.Show("Seleccione un método de pago.")
            Exit Sub
        End If

        ' Construir lista de platos
        Dim platos As String = ""
        For Each p In lstPedido.Items
            platos &= p.ToString().Split("-"c)(0).Trim() & ", "
        Next
        platos = platos.TrimEnd(" ", ",")

        Dim iva As Decimal = subtotal * 0.15D
        Dim total As Decimal = subtotal * 1.15D

        Dim mensaje As String =
            "Cliente: " & txtCliente.Text & vbCrLf &
            "Mesero: " & cmbMesero.SelectedItem.ToString() & vbCrLf &
            "Mesa: " & cmbMesa.SelectedItem.ToString() & vbCrLf &
            "Pedido: " & platos & vbCrLf &
            "Subtotal: " & subtotal.ToString("0.00") & vbCrLf &
            "IVA (15%): " & iva.ToString("0.00") & vbCrLf &
            "Total: " & total.ToString("0.00") & vbCrLf &
            "Método de pago: " & metodoPago & vbCrLf & vbCrLf &
            "Pedido registrado con éxito."

        MessageBox.Show(mensaje, "Confirmación")
    End Sub

    Private Sub btnNuevoPedido_Click(sender As Object, e As EventArgs) Handles btnNuevoPedido.Click
        txtCliente.Text = ""
        cmbMesero.SelectedIndex = -1
        cmbMesa.SelectedIndex = -1
        lstPedido.Items.Clear()
        subtotal = 0
        ActualizarTotales()
        rbEfectivo.Checked = False
        rbTarjeta.Checked = False
        txtNumeroTarjeta.Text = ""
        txtNumeroTarjeta.Enabled = False
        PicMesero.Image = Nothing
    End Sub
End Class
