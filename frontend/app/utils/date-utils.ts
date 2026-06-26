export const toApiDueDate = (value?: string) => {
    if (!value) {
        return null;
    }

    return `${value}T00:00:00.000Z`;
};

export const toInputDueDate = (value?: string | null) => {
    if (!value) {
        return "";
    }

    return value.slice(0, 10);
};
