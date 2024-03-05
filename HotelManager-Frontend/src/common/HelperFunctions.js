const buildQueryString = ({ filter, currentPage, pageSize, sortBy, sortOrder }) => {
    let queryString = '';
    queryString += `?pageNumber=${currentPage}&pageSize=${pageSize}`;
    if (sortBy) {
        queryString += `&sortBy=${sortBy}&sortOrder=${sortOrder}`;
    }
    for (const key in filter) {
        if (filter[key]) {
            queryString += `&${key}=${filter[key]}`;
        }
    }
    return queryString;
}

const formatDate = (dateString) => {
    const options = { day: 'numeric', month: 'numeric', year: 'numeric' };
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB', options);
};


export {
    buildQueryString,
    formatDate
}