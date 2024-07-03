var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    var status;
    if (url.includes('Approved')) {
        status = 'Approved'
    } else if (url.includes('Shipped')) {
        status = 'Shipped'
    } else if (url.includes('Processing')) {
        status = 'Processing'
    } else if (url.includes('ApprovedForDelayedPayment')) {
        status = 'ApprovedForDelayedPayment';
    } else {
        status='All'
    }
    loadDataTable(status);
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/Order/getall?status=' + status},
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "15%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/order/Detail?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>               
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}