$(function () {
    $("#users").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
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
            url: "/Admin/users/GetUserJsonData",
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

    $('#users').on('click', '.show-bs-modal', function () {
        let id = $(this).data("id");
        let deleteModal = $("#modal-delete");
        deleteModal.find('.modal-body p').text('Are you sure you want to delete this user?');
        $('#delete-id').val(id);
        $('#delete-form').attr('action', '/admin/users/delete');
        deleteModal.modal('show');
    });

    $('#delete-button').click(function () {
        $('#delete-form').submit();
    });

    // temp data response fadeOut
    $(document).ready(function () {
        setTimeout(function () {
            $('#response-alert').fadeOut('slow');
        }, 4000);
    });

    $('#users').on('click', '.showUpdateModal', function () {
        var userId = $(this).data('id');
        $.ajax({
            url: '/Admin/Users/GetUserForUpdate',
            type: 'GET',
            data: { id: userId },
            success: function (response) {
                if (response.success) {
                    var user = response.data;
                    $('#updateUserModal #Id').val(user.id);
                    $('#updateUserModal #FirstName').val(user.firstName);
                    $('#updateUserModal #LastName').val(user.lastName);
                    $('#updateUserModal #CompanyName').val(user.companyName);
                    $('#updateUserModal #Email').val(user.email);
                    $('#updateUserModal #PhoneNumber').val(user.phoneNumber);
                    $('#updateUserModal #Role').val(user.role);
                    $('#updateUserModal #Status').val(user.status);
                    $('#updateUserModal').modal('show');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while fetching user data.');
            }
        });
    });
    $('#update-button').click(function () {
        $('#updateUserForm').submit();
    });
});  
