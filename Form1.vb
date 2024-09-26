Public Class Form1
    Private productos As List(Of Producto)
    Private bindingSource As BindingSource
    Private nextId As Integer = 5 ' Para asignar ID a nuevos productos

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inicializar la lista de productos
        productos = New List(Of Producto) From {
            New Producto() With {.Id = 1, .Nombre = "Producto A", .Precio = 10D, .Categoria = "Electrónica"},
            New Producto() With {.Id = 2, .Nombre = "Producto B", .Precio = 15.5D, .Categoria = "Comida"},
            New Producto() With {.Id = 3, .Nombre = "Producto C", .Precio = 7.99D, .Categoria = "Electrónica"},
            New Producto() With {.Id = 4, .Nombre = "Producto D", .Precio = 12.5D, .Categoria = "Ropa"}
        }

        ' Configurar el BindingSource
        bindingSource = New BindingSource()
        bindingSource.DataSource = productos

        ' Enlace múltiple con DataGridView
        DataGridView1.DataSource = bindingSource
        DataGridView1.AutoGenerateColumns = True

        ' Enlace múltiple con BindingNavigator
        BindingNavigator1.BindingSource = bindingSource

        ' Llenar el ComboBox de categorías
        CargarCategorias()

        ' Enlace simple con TextBox y ComboBox para los detalles del producto seleccionado
        TextBoxNombre.DataBindings.Add("Text", bindingSource, "Nombre")
        TextBoxPrecio.DataBindings.Add("Text", bindingSource, "Precio")
        ComboBoxCategoria.DataBindings.Add("Text", bindingSource, "Categoria")
    End Sub

    ' Método para cargar categorías únicas en el ComboBox
    Private Sub CargarCategorias()
        ComboBoxCategoria.Items.Clear()
        ComboBoxCategoria.Items.Add("Todas las categorías")

        ' Obtener las categorías únicas de la lista de productos actualizada
        Dim categorias = productos.Select(Function(p) p.Categoria).Distinct()
        For Each categoria In categorias
            ComboBoxCategoria.Items.Add(categoria)
        Next

        ComboBoxCategoria.SelectedIndex = 0 ' Seleccionar "Todas las categorías" por defecto
    End Sub

    ' Evento para filtrar productos por categoría
    Private Sub ComboBoxCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxCategoria.SelectedIndexChanged
        Dim selectedCategory As String = ComboBoxCategoria.SelectedItem.ToString()

        If selectedCategory = "Todas las categorías" Then
            bindingSource.DataSource = productos
        Else
            bindingSource.DataSource = productos.Where(Function(p) p.Categoria = selectedCategory).ToList()
        End If

        bindingSource.ResetBindings(False)
    End Sub

    ' Evento para agregar un producto
    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles BtnAgregar.Click
        Dim nombre As String = InputBox("Ingrese el nombre del producto:")
        Dim precio As String = InputBox("Ingrese el precio del producto:")
        Dim categoria As String = InputBox("Ingrese la categoría del producto:")

        If Not String.IsNullOrEmpty(nombre) AndAlso Decimal.TryParse(precio, Nothing) AndAlso Not String.IsNullOrEmpty(categoria) Then
            Dim nuevoProducto As New Producto() With {
                .Id = nextId,
                .Nombre = nombre,
                .Precio = Decimal.Parse(precio),
                .Categoria = categoria
            }
            productos.Add(nuevoProducto)
            nextId += 1

            ' Actualizar el BindingSource
            bindingSource.ResetBindings(False)

            ' Actualizar ComboBox de categorías
            CargarCategorias()
        Else
            MessageBox.Show("Por favor, ingrese datos válidos.")
        End If
    End Sub

    ' Evento para eliminar un producto
    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If bindingSource.Current IsNot Nothing Then
            Dim producto As Producto = CType(bindingSource.Current, Producto)
            productos.Remove(producto)

            ' Actualizar el BindingSource
            bindingSource.ResetBindings(False)

            ' Actualizar ComboBox de categorías
            CargarCategorias()
        Else
            MessageBox.Show("Seleccione un producto para eliminar.")
        End If
    End Sub
End Class
