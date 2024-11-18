import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import { CategorySalaryScaleDTO } from "@models/CategorySalaryScaleDTO";

export const CategorySalaryScaleApisService = createApi({
    reducerPath: "CategorySalaryScaleApisService",
    tagTypes: ["CategorySalaryScaleApisService"],
    baseQuery: axiosBaseQuery(),
    endpoints: (builder) => ({
        GetListCategorySalaryScale: builder.query<ListResponse<CategorySalaryScaleDTO>, pagination>({
            query: ({ pageSize, pageNumber }): any => ({
                url: `/CategorySalaryScale/getListCategorySalaryScale`,
                params: { pageSize, pageNumber }
            }),
            providesTags(result) {
                if (result && result.listPayload) {
                    const { listPayload } = result;
                    return [
                        ...listPayload.map(({ id }) => ({ type: "CategorySalaryScaleApisService" as const, id })),
                        {
                            type: "CategorySalaryScaleApisService" as const,
                            id: "LIST"
                        }
                    ];
                }
                return [{ type: "CategorySalaryScaleApisService", id: "LIST" }];
            }
        }),
        UpdateCategorySalaryScale: builder.mutation<payloadResult, { CategorySalaryScale: Partial<CategorySalaryScaleDTO> }>({
            query: ({ CategorySalaryScale }) => ({
                url: `/CategorySalaryScale/updateCategorySalaryScale`,
                method: "PUT",
                data: CategorySalaryScale
            }),
            // invalidatesTags: (result, error, arg) => (result ?
            //   [{ type: "CategoryNationalityApisService", id: arg.CategoryNationality.id }] : [])
            invalidatesTags: (result, error, arg) => {
                if (result && arg && arg.CategorySalaryScale && arg.CategorySalaryScale.id) {
                    return [{ type: "CategorySalaryScaleApisService", id: arg.CategorySalaryScale.id }];
                }
                return [];
            }
        }),
        GetCategorySalaryScaleById: builder.query<ListResponse<CategorySalaryScaleDTO>, { idCategorySalaryScale: string }>({
            query: ({ idCategorySalaryScale }): any => ({
                url: `/CategorySalaryScale/getCategorySalaryScaleById`,
                params: { idCategorySalaryScale }
            }),
            providesTags: (result, error, arg) => [{ type: "CategorySalaryScaleApisService",
                id: arg.idCategorySalaryScale }]
        }),
        InsertCategorySalaryScale: builder.mutation<payloadResult, { CategorySalaryScale: Partial<CategorySalaryScaleDTO> }>({
            query: ({ CategorySalaryScale }) => ({
                url: `/CategorySalaryScale/insertCategoryTypeSalaryScale`,
                method: "POST",
                data: CategorySalaryScale
            }),
            invalidatesTags: (result) => (result ? [{ type: "CategorySalaryScaleApisService", id: "LIST" }] : [])
        }),
        DeleteCategorySalaryScale: builder.mutation<payloadResult, { idCategorySalaryScale: string[] }>({
            query: ({ idCategorySalaryScale }) => ({
                url: `/CategorySalaryScale/deleteCategorySalaryScale`,
                method: "DELETE",
                data: idCategorySalaryScale
            }),
            invalidatesTags: (result) => (result ? [{ type: "CategorySalaryScaleApisService", id: "LIST" }] : [])
        })


    })});
export const {
    useGetListCategorySalaryScaleQuery,
    useUpdateCategorySalaryScaleMutation,
    useGetCategorySalaryScaleByIdQuery,
    useInsertCategorySalaryScaleMutation,
    useDeleteCategorySalaryScaleMutation,
} = CategorySalaryScaleApisService;