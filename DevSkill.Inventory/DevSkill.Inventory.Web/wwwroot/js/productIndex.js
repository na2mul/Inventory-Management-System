$(function () {
    $("#products").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
        searching: false,
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, "All"]
        ],
        dom: '<"row mb-2"<"col-sm-12"l>>' +
            '<"row mb-2"<"col-sm-12"B>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row mt-2"<"col-sm-6"i><"col-sm-6"p>>',
        buttons: [
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            }
        ],

        ajax: { 
            url: "/Admin/Products/GetProductJsonData",
            type: "POST",
            contentType: "application/json", //must be added
            dataType: "json", //better to add, not mandatory
            data: function (d) {
                d.SearchItem = {
                    Name: $("#SearchItem_Name").val(),    
                    CategoryName: $("#SearchItem_CategoryName").val(),   
                    MeasurementUnitName: $("#SearchItem_MeasurementUnitName").val(),
                    Barcode: $("#SearchItem_Barcode").val(),
                    PurchasePriceFrom: $("#SearchItem_PurchasePriceFrom").val() === "" ? null : $("#SearchItem_PurchasePriceFrom").val(),
                    PurchasePriceTo: $("#SearchItem_PurchasePriceTo").val() === "" ? null : $("#SearchItem_PurchasePriceTo").val(),
                    StockFrom: $("#SearchItem_StockFrom").val() === "" ? null : $("#SearchItem_StockFrom").val(),
                    StockTo: $("#SearchItem_StockTo").val() === "" ? null : $("#SearchItem_StockTo").val()
                };
                return JSON.stringify(d);
            },
        },
        columnDefs: [{
            //use for left alignment view
            targets: "_all",
            className: 'dt-left'
        },
        {
            //use for showing sequential id number
            orderable: false,
            targets: 0,
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        {
            orderable: false,
            targets: 1
        },
        {
            orderable: false,
            targets: -1,
            render: function (data, type, row) {
                console.log(type);
                return `
                                <div class="d-flex justify-content-center">
                                    <div class="dropdown ms-auto">
                                                <i class="bi bi-three-dots" data-bs-toggle="dropdown"
                                                aria-expanded="false" role="button" title="Action"></i>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <button type="submit" class="dropdown-item showUpdateModal"
                                                        data-id='${data}' value='${data}' title="Edit">
                                                        <i class="default-color bi bi-pencil-square"></i>
                                                    Edit </button>
                                                </li>
                                                <li>
                                                    <button type="submit" class="dropdown-item
                                                        show-bs-modal" data-id='${data}' value='${data}'
                                                        title="Delete">
                                                        <i class="red-color bi bi-trash"></i>
                                                    Delete </button>
                                                </li>
                                            </ul>
                                        </div>
                                </div>`;
            }
        }
        ]
    });

    $('#products').on('click', '.show-bs-modal', function () {
        let id = $(this).data("id");
        let deleteModal = $("#modal-delete");
        deleteModal.find('.modal-body p').text('Are you sure you want to delete this product?');
        $('#delete-id').val(id);
        $('#delete-form').attr('action', '/admin/products/delete');
        deleteModal.modal('show');
    });

    $('#delete-button').click(function () {
        $('#delete-form').submit();
    });

    $(document).on('click', '.showUpdateModal', function () {
        const id = $(this).data('id');
        Update(id);
    });

    //Generate barcode
    document.querySelectorAll('.generate-barcode').forEach(btn => {
        btn.addEventListener('click', () => {
            const code = Math.floor(1e11 + Math.random() * 9e11);
            btn.closest('.modal')
                .querySelector('.barcode')
                .value = code;
        });
    });

    //Get Products
    function loadProducts($dropdown) {
        return $.getJSON('/Admin/Products/GetProducts')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select a Product*</option>');
                list.forEach(function (p) {
                    $dropdown.append(`<option value="${p.id}">${p.name}</option>`);
                }); 
                $dropdown.trigger('change');
            })
            .fail(function () {
                alert('Failed to load products.');
            });
    }

    // Searching Products for both Modals
    $('#damageProductModal, #storeProductModal').on('shown.bs.modal', function () {
        const $modal = $(this);            
        const $select = $modal.find('.product-select');

        if (!$select.hasClass('select2-hidden-accessible')) {
            loadProducts($select).then(() => {
                $select.select2({
                    allowClear: false,
                    width: '100%',
                    dropdownParent: $modal 
                });
            });
        }
    });
         
    $('#store-button').click(function () {
        $('#storeProductForm').submit();
    });

    $('#damage-button').click(function () {
        $('#damageProductForm').submit();
    });
  

    //Get Categories
    function loadCategories($dropdown, selectedId = '') {
        return $.getJSON('/Admin/Products/GetCategories')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select a category *</option>');
                list.forEach(function (c) {
                    $dropdown.append(`<option value="${c.id}">${c.categoryName}</option>`);
                });
                if (selectedId) {
                    $dropdown.val(selectedId).trigger('change');
                }
            })
            .fail(function () {
                alert('Failed to load categories.');
            });
    }

    //Get Units
    function loadMeasurementUnits($dropdown, selectedId = '') {
        return $.getJSON('/Admin/Products/GetMeasurementUnits')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select a measurement unit *</option>');

                list.forEach(function (u) {
                    $dropdown.append(`<option value="${u.id}">${u.name}</option>`);
                });

                if (selectedId) {
                    $dropdown.val(selectedId).trigger('change');
                }
            })
            .fail(function () {
                alert('Failed to load measurement units.');
            });
    }    

    //for update modal
    function Update(id) {
        const $modal = $('#updateProductModal');
        const $catSelect = $modal.find('.category-select');
        const $muSelect = $modal.find('.measurementUnit-select');

        $.ajax({
            url: '/Admin/Products/GetProductForUpdate',
            type: 'GET',
            data: { id },
            dataType: 'json'
        })
            .done(function (resp) {
                const p = resp.data || resp;

                $modal.find('#Id').val(p.id);
                $modal.find('#Name').val(p.name);
                $modal.find('#Barcode').val(p.barcode);
                $modal.find('#PurchasePrice').val(p.purchasePrice);
                $modal.find('#MRP').val(p.mrp);
                $modal.find('#WholesalePrice').val(p.wholesalePrice);
                $modal.find('#Stock').val(p.stock);
                $modal.find('#LowStock').val(p.lowStock);
                $modal.find('#DamageStock').val(p.damageStock);
                $modal.find('#Description').val(p.description);
                $modal.find('#ImageUrl').val(p.imageUrl);

                $.when(
                    loadCategories($catSelect, p.categoryId),
                    loadMeasurementUnits($muSelect, p.measurementUnitId)
                ).then(function () {
                    // Destroy existing instances if needed
                    if ($catSelect.hasClass('select2-hidden-accessible')) $catSelect.select2('destroy');
                    if ($muSelect.hasClass('select2-hidden-accessible')) $muSelect.select2('destroy');

                    $catSelect.select2({
                        tags: true,
                        allowClear: false,
                        width: '100%',
                        dropdownParent: $modal,
                        placeholder: 'Select or type a category',
                        createTag: params => {
                            const term = $.trim(params.term);
                            return term ? { id: term, text: term, newTag: true } : null;
                        }
                    });

                    $muSelect.select2({
                        tags: true,
                        allowClear: false,
                        width: '100%',
                        dropdownParent: $modal,
                        placeholder: 'Select or type a unit',
                        createTag: params => {
                            const term = $.trim(params.term);
                            return term ? { id: term, text: term, newTag: true } : null;
                        }
                    });

                    $modal.modal('show');
                });
            })
            .fail(function () {
                alert('Could not load product for editing.');
            });
    }


    // Add Product modal 
    $('#AddProductModal').on('show.bs.modal', function () {
        const $catSelect = $(this).find('.category-select');
        const $muSelect = $(this).find('.measurementUnit-select');

        loadCategories($catSelect);
        loadMeasurementUnits($muSelect);
    });

    $('#add-button').click(function () {
        $('#addProductForm').submit();
    });

    $('#update-button').click(function () {  
        $('#updateProductForm').submit();
    });    

    // Image Validation 
    document.addEventListener('change', function (e) {
        const input = e.target;
        if (!(input.matches && input.matches('input[type="file"]#Image'))) return;

        // Remove any previous error message
        const next = input.nextElementSibling;
        if (next && next.classList.contains('image-error')) next.remove();

        const file = input.files[0];
        if (!file) return;

        const allowed = ['image/png', 'image/jpeg'];
        const maxBytes = 512000;
        let message = '';

        if (!allowed.includes(file.type)) {
            message = 'PNG or JPG only';
        } else if (file.size > maxBytes) {
            message = 'Max Size 500 KB';
        }

        if (message) {
            const span = document.createElement('span');
            span.textContent = message;
            span.className = 'image-error text-danger small';
            // Insert the error message after the input 
            input.parentNode.insertBefore(span, input.nextSibling);
            // Clear the invalid file so user must reselect
            input.value = '';
        }
    });
  
    $('#SearchItem_PurchasePriceFrom').on('keyup', debounce(function () {
        console.log("Price from:" + $('#SearchItem_PurchasePriceFrom').val());
        $("#products").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_PurchasePriceTo').on('keyup', debounce(function () {
        console.log("Price to:" + $('#SearchItem_PurchasePriceTo').val());
        $("#products").DataTable().ajax.reload(null, false);
    }, 500));

    
    $('#SearchItem_CategoryName, #SearchItem_MeasurementUnitName, #SearchItem_Name,'+
        '#SearchItem_Barcode, #SearchItem_PurchasePriceFrom, #SearchItem_PurchasePriceTo,'+
        '#SearchItem_StockTo, #SearchItem_StockFrom').on('keyup', debounce(function () {
        $("#products").DataTable().ajax.reload(null, false);
    }, 500));

    function debounce(func, delay) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                func.apply(context, args);
            }, delay);
        };
    };

    $("#searchButton").click(function () {
        $('#products').DataTable().ajax.reload(null, false);
    });

    // temp data response fadeOut
    $(document).ready(function () {
        setTimeout(function () {
            $('#response-alert').fadeOut('slow');
        }, 4000);
    });

    // clear‑filter handler
    $(function () {
        const productsTable = $('#products').DataTable();

        $('#btnClearFilters').on('click', function () {

            //  clear every search field
            $('#SearchItem_Name, #SearchItem_CategoryName, #SearchItem_MeasurementUnitName, #SearchItem_Barcode,' +
                '#SearchItem_PurchasePriceFrom, #SearchItem_PurchasePriceTo, #SearchItem_StockTo, #SearchItem_StockFrom')
                .val('');

            // reset any Select2 (or similar) widgets
            $('.select2-hidden-accessible').val(null).trigger('change');

            // force the table to reload with no filters            
            productsTable.ajax.reload(null, true);
        });
    });

    // for category searching and selecting new category
    $(function () {
        $('#AddProductModal').on('shown.bs.modal', function () {
            const $modal = $(this);
            const $sel = $modal.find('.category-select').not('.select2-hidden-accessible');
            if (!$sel.length) return;

            loadCategories($sel).then(() => {
                $sel.select2({
                    tags: true,                     
                    allowClear: false,
                    width: '100%',
                    dropdownParent: $modal,
                    placeholder: 'Select or type a category',
                    createTag: params => {
                        const term = $.trim(params.term);
                        return term ? { id: term, text: term, newTag: true } : null;
                    }
                });
            });
        });
    });
    //for unit searching and selecting new unit
    $(function () {
        $('#AddProductModal').on('shown.bs.modal', function () {
            const $modal = $(this);
            const $sel = $modal.find('.measurementUnit-select').not('.select2-hidden-accessible');
            if (!$sel.length) return;

            loadMeasurementUnits($sel).then(() => {
                $sel.select2({
                    tags: true,
                    allowClear: false,
                    width: '100%',
                    dropdownParent: $modal,
                    placeholder: 'Select or type a unit',
                    createTag: params => {
                        const term = $.trim(params.term);
                        return term ? { id: term, text: term, newTag: true } : null;
                    }
                });
            });
        });
    });
   
});