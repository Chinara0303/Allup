$(document).ready(function () {
    $(document).on("click", "#deletetag", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response =>
                {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }
                    return response.text();
                }).then(data => {
                    $(".tagtable").html(data);
                })
                
            }
        })
    })
})

$(document).ready(function () {
    $(document).on("click", "#restoretag", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response =>
                {
                    if (response.ok) {
                        Swal.fire(
                            'restoredd!',
                            'Your file has been restored.',
                            'success'
                        )
                    }
                    return response.text();
                }).then(data => {
                    $(".tagtable").html(data);
                })

            }
        })
    })
})


   