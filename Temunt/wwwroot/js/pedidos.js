let listaProductos = [];

document.addEventListener('DOMContentLoaded', function () {
    if (!document.getElementById("detallePJson").value) {
        document.getElementById("detallePJson").value = "[]";
    }

    document.getElementById("btnAgregar").addEventListener("click", function () {
        let select = document.getElementById("productoSelect");
        let id_prod = parseInt(select.value);
        let nombre = select.options[select.selectedIndex].text;
        let precio = parseFloat(select.options[select.selectedIndex].getAttribute("data-precio"));
        let cantidad = parseInt(document.getElementById("cantidadInput").value);
        let garantia = document.getElementById("garantiaInput").value;

        if (!id_prod || cantidad <= 0 || isNaN(cantidad)) {
            alert("Selecciona un producto y una cantidad válida.");
            return;
        }

        let garantiaExp = null;
        if (garantia) {
            const fecha = new Date(garantia);
            garantiaExp = fecha.toISOString();
        }

        listaProductos.push({
            id_prod: id_prod,
            cantidad: cantidad,
            garantiaExp: garantiaExp
        });

        mostrarLista();
        limpiarFormulario();
        actualizarHiddenField();
    });

    document.getElementById("btnCrearPedido").addEventListener("click", function (e) {
        e.preventDefault();
        const clienteId = document.querySelector('[name="id_cliente"]').value;
        actualizarHiddenField();
        const currentJsonValue = document.getElementById("detallePJson").value;

        if (!clienteId) {
            alert("Selecciona un cliente");
            return;
        }

        if (listaProductos.length === 0) {
            alert("Agrega al menos un producto antes de crear el pedido.");
            return;
        }

        try {
            const parsed = JSON.parse(currentJsonValue);

            if (parsed.length === 0) {
                alert("Error: No hay productos para enviar.");
                return;
            }
        } catch (error) {
            alert("Error en los datos del pedido. Intenta nuevamente.");
            return;
        }

        if (confirm(`¿Estás seguro de crear el pedido con ${listaProductos.length} productos?`)) {
            e.target.closest("form").submit();
        }
    });
});

function limpiarFormulario() {
    document.getElementById("productoSelect").value = "";
    document.getElementById("cantidadInput").value = "";
    document.getElementById("garantiaInput").value = "";
}

function mostrarLista() {
    let contenedor = document.getElementById("listaProductos");
    if (!contenedor) {
        console.error("No se encontró el contenedor listaProductos");
        return;
    }

    contenedor.innerHTML = "";
    let total = 0;
    let totalProductos = 0;

    if (listaProductos.length === 0) {
        contenedor.innerHTML = "<p class='text-muted'>No hay productos agregados</p>";
    } else {
        listaProductos.forEach(function (p, index) {
            let productoSelect = document.getElementById("productoSelect");
            let precio = 0;
            let productoNombre = "Producto no encontrado";

            for (let option of productoSelect.options) {
                if (option.value == p.id_prod) {
                    precio = parseFloat(option.getAttribute("data-precio"));
                    productoNombre = option.text;
                    break;
                }
            }

            total += p.cantidad * precio;
            totalProductos += p.cantidad;

            contenedor.innerHTML += `
            <div class="d-flex justify-content-between align-items-center mb-2 p-2 border rounded">
                <div>
                    <strong>${productoNombre}</strong><br>
                    <small>Cantidad: ${p.cantidad} | Garantía: ${p.garantiaExp ? new Date(p.garantiaExp).toLocaleDateString() : 'N/A'}</small>
                </div>
                <div class="text-end">
                    <div>$${(p.cantidad * precio).toFixed(2)}</div>
                    <button type="button" class="btn btn-sm btn-danger mt-1" onclick="eliminarProducto(${index})">Eliminar</button>
                </div>
            </div>`;
        });
    }

    document.getElementById("totalProductos").textContent = totalProductos;
    document.getElementById("totalPedido").textContent = total.toFixed(2);
}

function eliminarProducto(index) {
    listaProductos.splice(index, 1);
    mostrarLista();
    actualizarHiddenField();
}

function actualizarHiddenField() {
    const detallesParaEnviar = listaProductos.map(function (p) {
        return {
            id_prod: p.id_prod,
            cantidad: p.cantidad,
            garantiaExp: p.garantiaExp
        };
    });

    const jsonString = JSON.stringify(detallesParaEnviar);
    document.getElementById("detallePJson").value = jsonString;
}