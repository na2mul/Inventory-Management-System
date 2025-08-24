$(function () {
    // Global variable to store products
    var saleProducts = [];

    $("#sales").DataTable({
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
                extend: 'copyHtml5',
                text: '<i class="fas fa-copy"></i> Copy',
                className: 'btn btn-default',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fas fa-file-csv"></i> CSV',
                className: 'btn btn-default',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="fas fa-file-excel"></i> Excel',
                className: 'btn btn-default',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf"></i> PDF',
                className: 'btn btn-default',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Print',
                className: 'btn btn-default',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            }
        ],

        ajax: {
            url: "/Admin/Sales/GetSaleJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                function formatDateTime(selector) {
                    const val = $(selector).val();
                    if (!val) return null;
                    const date = new Date(val);
                    return date.toISOString();
                }
                d.SearchItem = {
                    InvoiceNo: $("#SearchItem_InvoiceNo").val(),
                    CustomerName: $("#SearchItem_CustomerName").val(),
                    SaleDateFrom: formatDateTime("#SearchItem_SaleDateFrom"),
                    SaleDateTo: formatDateTime("#SearchItem_SaleDateTo"),
                    TotalPriceFrom: $("#SearchItem_TotalPriceFrom").val() === "" ? null : $("#SearchItem_TotalPriceFrom").val(),
                    TotalPriceTo: $("#SearchItem_TotalPriceTo").val() === "" ? null : $("#SearchItem_TotalPriceTo").val(),
                    PaidAmount: $("#SearchItem_PaidAmount").val() === "" ? null : $("#SearchItem_PaidAmount").val(),
                    DueAmount: $("#SearchItem_DueAmount").val() === "" ? null : $("#SearchItem_DueAmount").val(),
                    Status: $("#SearchItem_Status").val()
                };
                return JSON.stringify(d);
            },
        },
        columnDefs: [{
            targets: "_all",
            className: 'dt-left'
        },
        {
            orderable: false,
            targets: 0,
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        {
            orderable: false,
            targets: -1,
            render: function (data, type, row) {
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
                                    <button type="submit" class="dropdown-item show-bs-modal" 
                                        data-id='${data}' value='${data}' title="Delete">
                                        <i class="red-color bi bi-trash"></i>
                                    Delete </button>
                                </li>
                            </ul>
                        </div>
                    </div>`;
            }
        }]
    });

    // Delete modal handling
    $('#sales').on('click', '.show-bs-modal', function () {
        let id = $(this).data("id");
        let deleteModal = $("#modal-delete");
        deleteModal.find('.modal-body p').text('Are you sure you want to delete this sale?');
        $('#delete-id').val(id);
        $('#delete-form').attr('action', '/admin/sales/delete');
        deleteModal.modal('show');
    });

    $('#delete-button').click(function () {
        $('#delete-form').submit();
    });

    // Image Validation 
    document.addEventListener('change', function (e) {
        const input = e.target;
        if (!(input.matches && input.matches('input[type="file"]#Image'))) return;

        const existing = input.parentNode.querySelector('.image-error');
        if (existing) existing.remove();

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
            input.insertAdjacentElement('afterend', span);
            input.value = '';
        }
    });

    // Load Products
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

    // Initialize Products Select2
    $('#addSaleModal').on('shown.bs.modal', function () {
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

    // Load Customers
    function loadCustomers($dropdown) {
        return $.getJSON('/Admin/Customers/GetCustomers')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select a Customer*</option>');
                list.forEach(function (c) {
                    $dropdown.append(`<option value="${c.id}">${c.name}</option>`);
                });
                $dropdown.trigger('change');
            })
            .fail(function () {
                alert('Failed to load customers.');
            });
    }

    // Initialize Customers Select2
    $('#addSaleModal').on('shown.bs.modal', function () {
        const $modal = $(this);
        const $select = $modal.find('.customer-select');

        if (!$select.hasClass('select2-hidden-accessible')) {
            loadCustomers($select).then(() => {
                $select.select2({
                    allowClear: false,
                    width: 'resolve',
                    dropdownParent: $modal
                });
            });
        }
    });

    // Load AccountTypes
    function accountTypes($dropdown) {
        return $.getJSON('/Admin/Sales/GetAccountTypes')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select a AccountType*</option>');
                list.forEach(function (at) {
                    $dropdown.append(`<option value="${at.id}">${at.name}</option>`);
                });
                $dropdown.trigger('change');
            })
            .fail(function () {
                alert('Failed to load AccountTypes.');
            });
    }

    // Initialize AccountTypes Select2
    $('#addSaleModal').on('shown.bs.modal', function () {
        const $modal = $(this);
        const $select = $modal.find('.accountType-select');

        if (!$select.hasClass('select2-hidden-accessible')) {
            accountTypes($select).then(() => {
                $select.select2({
                    allowClear: false,
                    width: 'resolve',
                    dropdownParent: $modal
                });
            });
        }
    });

    // Function to load accounts based on account type
    function loadAccountsByType(accountTypeId, $dropdown) {
        // Clear the accounts dropdown
        $dropdown.empty().append('<option value="">Loading accounts...</option>');
        $dropdown.prop('disabled', true);

        if (!accountTypeId || accountTypeId === '') {
            $dropdown.empty().append('<option value="">Select Account Type First</option>');
            $dropdown.prop('disabled', true);
            return;
        }

        // Make AJAX call to get accounts by type
        $.ajax({
            url: '/Admin/Sales/GetAccountsByType',
            type: 'GET',
            data: { accountTypeId: accountTypeId },
            dataType: 'json',
            success: function (accounts) {
                // Clear loading message
                $dropdown.empty();

                if (accounts && accounts.length > 0) {
                    // Add default option
                    $dropdown.append('<option value="">Select Account</option>');

                    // Add accounts to dropdown
                    accounts.forEach(function (account) {
                        $dropdown.append(`<option value="${account.id}">${account.accountName}</option>`);
                    });

                    // Enable the dropdown
                    $dropdown.prop('disabled', false);

                    console.log(`Loaded ${accounts.length} accounts for account type ${accountTypeId}`);
                } else {
                    // No accounts found
                    $dropdown.append('<option value="">No accounts available</option>');
                    $dropdown.prop('disabled', true);
                    console.log(`No accounts found for account type ${accountTypeId}`);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error loading accounts:', error);
                $dropdown.empty().append('<option value="">Error loading accounts</option>');
                $dropdown.prop('disabled', true);

                // Show user-friendly error message
                alert('Failed to load accounts. Please try again.');
            }
        });
    }

    
    $(document).ready(function () {
        var allAccounts = [];

        $('#AccountTypeId').change(function () {
            var accountTypeId = $(this).val();
            var $accountDropdown = $('#AccountId');

            console.log('Account type changed to:', accountTypeId);

            // Load accounts based on selected type
            loadAccountsByType(accountTypeId, $accountDropdown);
        });

        // Reset account dropdown when modal is closed
        $('#addSaleModal').on('hidden.bs.modal', function () {
            $('#AccountId').empty().append('<option value="">Select Account</option>');
            $('#AccountTypeId').val('');
        });

        // Initialize account dropdown state
        $('#AccountId').empty().append('<option value="">Select Account Type First</option>');
        $('#AccountId').prop('disabled', true);
    });

    // Temp data response fadeOut
    $(document).ready(function () {
        setTimeout(function () {
            $('#response-alert').fadeOut('slow');
        }, 4000);
    });

    // Product selection and adding to table
    $('#productId').change(function () {
        var id = $('#productId').val();
        if (!id || id === '' || id === '0' || id === 'null' || id === 'undefined') {
            return;
        }

        // Check if product is already added
        var existingProduct = saleProducts.find(p => p.ProductId === id);
        if (existingProduct) {
            alert('Product is already added');
            $('#productId').val('').trigger('change');
            return;
        }

        var saleType = $('#saleType').val();
        var url = '/Admin/Sales/GetProductDetails?id=' + id + '&saleType=' + saleType;

        $.ajax({
            type: 'GET',
            url: url,
            dataType: 'text',
            success: function (data) {
                if (data == "1") {
                    alert('Product not found');
                }
                else if (data == "2") {
                    alert('Product stock out');
                }
                else if (data == "3") {
                    alert('Error in GetProductDetails');
                }
                else if (data == "0") {
                    alert('Sale type required');
                }
                else {
                    var jsondata = JSON.parse(data);
                    var saleType = $('#saleType').val();
                    var saleTypeText = saleType === '1' ? 'MRP' : 'Wholesale';


                    var product = {
                        ProductId: jsondata.Id.toString(),
                        Barcode: jsondata.Barcode || '',
                        ProductName: jsondata.Name || '',
                        Quantity: parseInt(jsondata.Quantity) || 1,
                        UnitPrice: parseFloat(jsondata.Price) || 0,
                        SubTotal: parseFloat(jsondata.Subtotal) || 0,
                        StockAvailable: parseInt(jsondata.Stock) || 0,
                        SaleType: saleTypeText
                    };

                    saleProducts.push(product);

                    // Create HTML table row
                    var tableRow = `
                    <tr data-product-id="${product.ProductId}">
                        <td>${product.Barcode}</td>
                        <td>${product.ProductName}</td>
                        <td>${product.StockAvailable}</td>
                        <td>
                            <input type="number" class="form-control quantity-input" 
                                   value="${product.Quantity}" min="1" max="${product.StockAvailable}" 
                                   data-price="${product.UnitPrice}" data-product-id="${product.ProductId}">
                        </td>
                        <td>${product.UnitPrice.toFixed(2)}</td>
                        <td class="subtotal">${product.SubTotal.toFixed(2)}</td>
                        <td>
                            <button type="button" class="btn btn-danger btn-sm remove-btn" data-product-id="${product.ProductId}">
                                <i class="fa fa-trash"></i>
                            </button>
                        </td>
                    </tr>
                `;
                    $('#tbody').append(tableRow);
                    $('#productId').val('').trigger('change');
                    calculatePrice();
                    updateProductsHiddenFields();
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching product details:', error);
                alert('Error loading product details. Please try again.');
            }
        });
    });

    // Event delegation for dynamically added elements
    $(document).on('input', '.quantity-input', function () {
        updateSubtotal(this);
    });

    $(document).on('click', '.remove-btn', function () {
        removeRow(this);
    });

    // Update subtotal when quantity changes
    function updateSubtotal(element) {
        var $row = $(element).closest('tr');
        var quantity = parseInt($(element).val()) || 0;
        var price = parseFloat($(element).data('price')) || 0;
        var maxStock = parseInt($(element).attr('max')) || 0;
        var productId = $(element).data('product-id').toString(); // Ensure it's a string

        // Validate quantity doesn't exceed stock
        if (quantity > maxStock) {
            alert('Quantity cannot exceed available stock (' + maxStock + ')');
            $(element).val(maxStock);
            quantity = maxStock;
        }

        // Calculate subtotal
        var subtotal = quantity * price;

        // Update the subtotal cell in the same row
        $row.find('.subtotal').text(subtotal.toFixed(2));

        // Update the product in the array (compare as strings)
        var product = saleProducts.find(p => p.ProductId.toString() === productId);
        if (product) {
            product.Quantity = quantity;
            product.SubTotal = subtotal;
        }

        // Recalculate total price
        calculatePrice();
        updateProductsHiddenFields();
    }

    // Remove a row
    function removeRow(element) {
        if (confirm('Are you sure you want to remove this product?')) {
            var productId = $(element).data('product-id').toString(); // Ensure it's a string

            // Remove from products array (compare as strings)
            saleProducts = saleProducts.filter(p => p.ProductId.toString() !== productId);

            // Remove from table
            $(element).closest('tr').remove();
            calculatePrice();
            updateProductsHiddenFields();

            // If no products left, reset totals
            if (saleProducts.length === 0) {
                resetTotals();
            }
        }
    }

    // Update products hidden fields for proper model binding
    function updateProductsHiddenFields() {
        $('#productsContainer').empty();
        saleProducts.forEach(function (p, i) {
            $('#productsContainer').append(`
                <input type="hidden" name="Products[${i}].ProductId" value="${p.ProductId}" />
                <input type="hidden" name="Products[${i}].Barcode" value="${p.Barcode}" />
                <input type="hidden" name="Products[${i}].Quantity" value="${p.Quantity}" />
                <input type="hidden" name="Products[${i}].UnitPrice" value="${p.UnitPrice}" />
                <input type="hidden" name="Products[${i}].SubTotal" value="${p.SubTotal}" />
                <input type="hidden" name="Products[${i}].StockAvailable" value="${p.StockAvailable}" />
                <input type="hidden" name="Products[${i}].SaleType" value="${p.SaleType}" />
            `);
        });
    }

    // Calculate total price and update BOTH display AND hidden fields
    function calculatePrice() {
        var totalSubtotal = 0;

        // Sum all subtotals from products array
        saleProducts.forEach(function (product) {
            totalSubtotal += parseFloat(product.SubTotal) || 0;
        });

        // Update the total subtotal (before VAT and discount)
        $('#tsAmount').val(totalSubtotal);

        // Calculate VAT
        var vatPercent = parseFloat($('#vCost').val()) || 0;
        var vatAmount = (totalSubtotal * vatPercent) / 100;
        $('#vAmount').val(vatAmount);

        // Calculate net amount (subtotal + VAT) - ROUND TO INTEGER FOR SERVER
        var netAmountDecimal = totalSubtotal + vatAmount;
        var netAmountInteger = Math.round(netAmountDecimal);

        // Calculate discount based on INTEGER net amount
        var discountPercent = parseFloat($('#discount').val()) || 0;
        var discountAmountDecimal = (netAmountInteger * discountPercent) / 100;
        var discountAmountInteger = Math.round(discountAmountDecimal);
        $('#disAmount').val(discountAmountDecimal);

        // Calculate final total (INTEGER net amount - INTEGER discount)
        var totalAmountInteger = netAmountInteger - discountAmountInteger;

        // Calculate due amount
        var paidAmount = parseFloat($('#total_paid').val()) || 0;
        var paidAmountInteger = Math.round(paidAmount);
        var dueAmountInteger = totalAmountInteger - paidAmountInteger;

        // Update DISPLAY fields (keep decimals for user interface)
        $('#nAmount').val(netAmountDecimal.toFixed(2));
        $('#totalprice').val(totalAmountInteger.toFixed(2)); // Show integer as decimal format
        $('#total_remain').val(dueAmountInteger.toFixed(2)); // Show integer as decimal format

        // Update HIDDEN fields that will be submitted to server - ALL AS INTEGERS
        $('#vatAmountHidden').val(Math.round(vatAmount));
        $('#netAmountHidden').val(netAmountInteger);
        $('#discountHidden').val(discountAmountInteger);
        $('#totalAmountHidden').val(totalAmountInteger);
        $('#dueAmountHidden').val(dueAmountInteger);
        $('#paidAmountHidden').val(paidAmountInteger);

        // Debug logging
        console.log('Calculation Debug:', {
            totalSubtotal: totalSubtotal,
            vatAmount: vatAmount,
            netAmountDecimal: netAmountDecimal,
            netAmountInteger: netAmountInteger,
            discountAmountDecimal: discountAmountDecimal,
            discountAmountInteger: discountAmountInteger,
            totalAmountInteger: totalAmountInteger,
            paidAmountInteger: paidAmountInteger,
            dueAmountInteger: dueAmountInteger,
            hiddenValues: {
                vatAmountHidden: $('#vatAmountHidden').val(),
                netAmountHidden: $('#netAmountHidden').val(),
                discountHidden: $('#discountHidden').val(),
                totalAmountHidden: $('#totalAmountHidden').val(),
                dueAmountHidden: $('#dueAmountHidden').val(),
                paidAmountHidden: $('#paidAmountHidden').val()
            }
        });
    }

    // Reset all totals
    function resetTotals() {
        // Reset display fields
        $('#tsAmount').val(0);
        $('#vAmount').val(0);
        $('#nAmount').val(0);
        $('#disAmount').val(0);
        $('#totalprice').val(0);
        $('#total_paid').val(0);
        $('#total_remain').val(0);
        $('#vCost').val(0);
        $('#discount').val(0);

        // Reset hidden fields
        $('#vatAmountHidden').val(0);
        $('#netAmountHidden').val(0);
        $('#discountHidden').val(0);
        $('#totalAmountHidden').val(0);
        $('#dueAmountHidden').val(0);
        $('#paidAmountHidden').val(0);
    }

    // VAT calculation function (called from HTML)
    window.vatcostcalculator = function () {
        calculatePrice();
    }

    // Discount calculation function (called from HTML)
    window.discountType = function () {
        calculatePrice();
    }

    // Calculate remaining amount function (called from HTML)
    window.calculate_remain = function () {
        calculatePrice(); // Recalculate everything when paid amount changes
    }

    // Form validation
    function validateSaleForm() {
        if (saleProducts.length === 0) {
            alert('Please add at least one product to the sale');
            return false;
        }

        var customerId = $('#CustomerId').val();
        if (!customerId) {
            alert('Please select a customer');
            return false;
        }

        var saleType = $('#saleType').val();
        if (!saleType) {
            alert('Please select a sale type');
            return false;
        }

        var accountTypeId = $('#AccountTypeId').val();
        if (!accountTypeId) {
            alert('Please select an account type');
            return false;
        }

        var accountId = $('#AccountId').val();
        if (!accountId) {
            alert('Please select an account');
            return false;
        }

        return true;
    }

    // Form submission handling with proper debugging
    $('#saleForm').on('submit', function (e) {
        console.log('Form submission started...');

        if (!validateSaleForm()) {
            e.preventDefault();
            console.log('Form validation failed');
            return false;
        }

        // Ensure calculations are up to date before submission
        calculatePrice();

        // Update products hidden fields before submission
        updateProductsHiddenFields();

        // Generate invoice number if not set
        if (!$('#invoiceNo').val()) {
            var invoiceNo = 'INV-DEV' + new Date().getTime();
            $('#invoiceNo').val(invoiceNo);
        }

        // Set status if not set
        if (!$('#status').val()) {
            var paidAmount = parseFloat($('#total_paid').val()) || 0;
            var totalAmount = parseFloat($('#totalAmountHidden').val()) || 0;

            if (paidAmount >= totalAmount) {
                $('#status').val('Full Paid');
            } else if (paidAmount > 0) {
                $('#status').val('Partial Paid');
            } else {
                $('#status').val('Due');
            }
        }

        // ENSURE AMOUNTS ARE SET AS INTEGERS BEFORE SUBMISSION
        var paidAmountInteger = Math.round(parseFloat($('#total_paid').val()) || 0);
        var netAmountInteger = parseInt($('#netAmountHidden').val()) || 0;
        var totalAmountInteger = parseInt($('#totalAmountHidden').val()) || 0;

        // Remove existing hidden fields if they exist and add new ones
        $('#paidAmountHidden').remove();
        $('#netAmountHidden').remove();
        $('#totalAmountHidden').remove();

        // Add integer hidden fields for model binding
        $('<input>').attr({
            type: 'hidden',
            id: 'paidAmountHidden',
            name: 'PaidAmount', // Make sure this matches your model property name
            value: paidAmountInteger
        }).appendTo('#saleForm');

        $('<input>').attr({
            type: 'hidden',
            id: 'netAmountHidden',
            name: 'NetAmount', // Make sure this matches your model property name
            value: netAmountInteger
        }).appendTo('#saleForm');

        $('<input>').attr({
            type: 'hidden',
            id: 'totalAmountHidden',
            name: 'TotalAmount', // Make sure this matches your model property name
            value: totalAmountInteger
        }).appendTo('#saleForm');

        // Log all form data being submitted
        console.log('Products being submitted:', saleProducts);
        console.log('Integer Values:', {
            paidAmount: paidAmountInteger,
            netAmount: netAmountInteger,
            totalAmount: totalAmountInteger
        });
        console.log('Form data being submitted:');

        var formData = new FormData(this);
        for (var pair of formData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);
        }

        // Log serialized form data
        console.log('Serialized form data:', $(this).serialize());

        console.log('Form submission proceeding...');
        return true;
    });

    // Initialize modal when shown
    $('#addSaleModal').on('shown.bs.modal', function () {
        console.log('Modal opened - initializing form');

        // Reset form and arrays
        saleProducts = [];
        $('#tbody').empty();
        $('#productsContainer').empty();

        // Set default sale date to current date/time
        var now = new Date();
        var formattedDate = now.getFullYear() + '-' +
            String(now.getMonth() + 1).padStart(2, '0') + '-' +
            String(now.getDate()).padStart(2, '0') + 'T' +
            String(now.getHours()).padStart(2, '0') + ':' +
            String(now.getMinutes()).padStart(2, '0');
        $('#saleDate').val(formattedDate);

        // Initialize with zero values
        resetTotals();
    });

    // Event delegation for VAT and discount inputs
    $(document).on('input change', '#vCost', function () {
        calculatePrice();
    });

    $(document).on('input change', '#discount', function () {
        calculatePrice();
    });

    $(document).on('input change', '#total_paid', function () {
        calculatePrice(); // Recalculate everything when paid amount changes
    });

    // Initialize when page loads
    $(document).ready(function () {
        console.log('Sale form JavaScript initialized with proper form submission handling');
    });
});