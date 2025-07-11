$(function () {
    $("#customers").DataTable({
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
        ajax: {
            url: "/Admin/Customers/GetCustomerJsonData",
            type: "POST",
            contentType: "application/json", //must be added
            dataType: "json", //better to add, not mandatory
            data: function (d) {
                d.SearchItem = {
                    Name: $("#SearchItem_Name").val(),
                    CustomerId: $("#SearchItem_CustomerId").val(),
                    Mobile: $("#SearchItem_Mobile").val(),
                    Address: $("#SearchItem_Address").val(),
                    Email: $("#SearchItem_Email").val(),
                    BalanceFrom: $("#SearchItem_BalanceFrom").val() === "" ? null : $("#SearchItem_BalanceFrom").val(),
                    BalanceTo: $("#SearchItem_BalanceTo").val() === "" ? null : $("#SearchItem_BalanceTo").val()
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
        }]
    });

    $('#customers').on('click', '.show-bs-modal', function () {
        let id = $(this).data("id");
        let deleteModal = $("#modal-delete");
        deleteModal.find('.modal-body p').text('Are you sure you want to delete this customer?');
        $('#delete-id').val(id);
        $('#delete-form').attr('action', '/admin/customers/delete');
        deleteModal.modal('show');
    });

    $('#delete-button').click(function () {
        $('#delete-form').submit();
    });

    $('#add-button').click(function () {
        $('#addCustomerForm').submit();
    });

    $('#update-button').click(function () {
        $('#updateCustomerForm').submit();
    });
});