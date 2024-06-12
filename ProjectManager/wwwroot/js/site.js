function makeEmployeeSearchBox(selectBox, action) {
    defaultSearchBox(selectBox, action, "Search employee...", (data) => {
        var results = []
        data.forEach((client) => {
            results.push({
                id: client.id,
                text: [client.firstName, client.middleName, client.lastName].join(' ') + " - " + client.email
            })
        })

        return { results: results }
    })
}

function makeProjectSearchBox(selectBox, action) {
    defaultSearchBox(selectBox, action, "Search project...", (data) => {
        var results = []
        data.forEach((project) => {
            results.push({
                id: project.id,
                text: project.name
            })
        })

        return { results: results }
    })
}

function defaultSearchBox(selectBox, action, placeholder, processResult) {
    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 2,
        ajax: {
            url: action,
            type: "GET",
            dataType: 'json',
            quietMillis: 100,
            data: (params) => {
                return { query: params.term }
            },
            processResults: processResult
        }
    })
}
