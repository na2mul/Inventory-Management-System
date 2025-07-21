$(function () {
    $("#transferAccount").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, "All"]
        ],
        dom: '<"row mb-2"<"col-sm-12"l><"col-sm-12 text-end"f>>' +
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
            url: "/Admin/TransferAccounts/GetTransferAccountJsonData",
            type: "POST",
            contentType: "application/json", //must be added
            dataType: "json", //better to add, not mandatory
            data: function (d) {
                d.SearchItem = {
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

    $('#transferAccount').on('click', '.show-bs-modal', function () {
        let id = $(this).data("id");
        let deleteModal = $("#modal-delete");
        deleteModal.find('.modal-body p').text('Are you sure you want to delete this TransferAccount?');
        $('#delete-id').val(id);
        $('#delete-form').attr('action', '/admin/transferAccounts/delete');
        deleteModal.modal('show');
    });

    $('#delete-button').click(function () {
        $('#delete-form').submit();
    });

    // Load AccountTypes
    function accountTypes($dropdown) {
        return $.getJSON('/Admin/Sales/GetAccountTypes')
            .done(function (list) {
                $dropdown.empty().append('<option value="">Select one*</option>');
                list.forEach(function (at) {
                    $dropdown.append(`<option value="${at.id}">${at.name}</option>`);
                });
                $dropdown.trigger('change');
            })
            .fail(function () {
                alert('Failed to load AccountTypes.');
            });
    }

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
                $dropdown.empty();
                if (accounts && accounts.length > 0) {                    
                    $dropdown.append('<option value="">Select Account</option>');                    
                    accounts.forEach(function (account) {
                        $dropdown.append(`<option value="${account.id}">${account.accountName}</option>`);
                    });                    
                    $dropdown.prop('disabled', false);
                    console.log(`Loaded ${accounts.length} accounts for account type ${accountTypeId}`);
                } else {                    
                    $dropdown.append('<option value="">No accounts available</option>');
                    $dropdown.prop('disabled', true);
                    console.log(`No accounts found for account type ${accountTypeId}`);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error loading accounts:', error);
                $dropdown.empty().append('<option value="">Error loading accounts</option>');
                $dropdown.prop('disabled', true);                
                alert('Failed to load accounts. Please try again.');
            }
        });
    }

    // Initialize AccountTypes Select2 when modal is shown
    $('#addBalanceTransferModal').on('shown.bs.modal', function () {
        const $modal = $(this);

        // Set current datetime to hidden field
        const now = new Date();
        const currentDateTime = now.toISOString().slice(0, 19); // Format: YYYY-MM-DDTHH:mm:ss
        $modal.find('input[name="TransferDate"]').val(currentDateTime);

        // Initialize sender account type dropdown
        const $senderAccountType = $modal.find('#SenderAccountTypeId');
        if (!$senderAccountType.hasClass('select2-hidden-accessible')) {
            accountTypes($senderAccountType).then(() => {
                $senderAccountType.select2({
                    allowClear: false,
                    width: 'resolve',
                    dropdownParent: $modal
                });
            });
        }

        // Initialize receiver account type dropdown
        const $receiverAccountType = $modal.find('#ReceiverAccountTypeId');
        if (!$receiverAccountType.hasClass('select2-hidden-accessible')) {
            accountTypes($receiverAccountType).then(() => {
                $receiverAccountType.select2({
                    allowClear: false,
                    width: 'resolve',
                    dropdownParent: $modal
                });
            });
        }
    });

    $(document).ready(function () {
        // Handle sender account type change
        $(document).on('change', '#SenderAccountTypeId', function () {
            var accountTypeId = $(this).val();
            var $accountDropdown = $('#FromAccountId');
            console.log('Sender account type changed to:', accountTypeId);

            // Load accounts based on selected type
            loadAccountsByType(accountTypeId, $accountDropdown);
        });

        // Handle receiver account type change
        $(document).on('change', '#ReceiverAccountTypeId', function () {
            var accountTypeId = $(this).val();
            var $accountDropdown = $('#ToAccountId');
            console.log('Receiver account type changed to:', accountTypeId);

            // Load accounts based on selected type
            loadAccountsByType(accountTypeId, $accountDropdown);
        });

        // Reset dropdowns when modal is closed
        $('#addBalanceTransferModal').on('hidden.bs.modal', function () {
            // Reset sender dropdowns
            $('#FromAccountId').empty().append('<option value="">Select Account</option>');
            $('#SenderAccountTypeId').val('').trigger('change');

            // Reset receiver dropdowns
            $('#ToAccountId').empty().append('<option value="">Select Account</option>');
            $('#ReceiverAccountTypeId').val('').trigger('change');

            // Destroy Select2 instances to prevent memory leaks
            if ($('#SenderAccountTypeId').hasClass('select2-hidden-accessible')) {
                $('#SenderAccountTypeId').select2('destroy');
            }
            if ($('#ReceiverAccountTypeId').hasClass('select2-hidden-accessible')) {
                $('#ReceiverAccountTypeId').select2('destroy');
            }
        });

        // Initialize account dropdowns state on page load
        $('#FromAccountId').empty().append('<option value="">Select Account Type First</option>');
        $('#FromAccountId').prop('disabled', true);

        $('#ToAccountId').empty().append('<option value="">Select Account Type First</option>');
        $('#ToAccountId').prop('disabled', true);
    });

    $("#searchButton").click(function () {
        $('#transferAccount').DataTable().ajax.reload(null, false);
    });
});