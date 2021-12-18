export interface ISearchState {
    journals: IResult[];
    selectedJournal: IJournal;
    isLoading: boolean;
    error: string;
}

export interface IResult {
    document: IJournal;
    score: number;
}

export interface IJournal {
    id: number;
    title: string;
    about: string;
    url: string;
    editorial: string;
    metrics: IMetric[];
    imgUrl: string;
}

export interface IMetric {
    id: number;
    name: string;
    value: number;
}


export interface IRefineField {
    name: string;
    active: boolean;
    boost: number;
}

export interface IRefineItem {
    title: string;
    fields: IRefineField[];
}
