
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
        ajax: {
            url: "/Admin/Products/GetProductJsonData",
            type: "POST",
            contentType: "application/json", //must be added
            dataType: "json", //better to add, not mandatory
            data: function (d) {
                d.SearchItem = {
                    Name: $("#SearchItem_Name").val(),
                    Description: $("#SearchItem_Description").val(),
                    PriceFrom: $("#SearchItem_PriceFrom").val() === "" ? null : $("#SearchItem_PriceFrom").val(),
                    PriceTo: $("#SearchItem_PriceTo").val() === "" ? null : $("#SearchItem_PriceTo").val()
                };
                return JSON.stringify(d);
            },
        },
        columnDefs: [
            {
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

$('#delete-button').click(function()
{
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

//update
function Update(id) {
            const $modal     = $('#updateProductModal');
const $catSelect = $modal.find('.category-select');
const $muSelect  = $modal.find('.measurementUnit-select');

$.ajax({
    url: '/Admin/Products/GetProductForUpdate',
type: 'GET',
data: {id},
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
const $muSelect  = $(this).find('.measurementUnit-select');

loadCategories($catSelect);
loadMeasurementUnits($muSelect); 
        });

$('#add-button').click(function()
{
    $('#addProductForm').submit();
        });

$('#update-button').click(function()
{
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

$("#searchButton").click(function () {
    $('#products').DataTable().ajax.reload(null, false);
        });

    });
