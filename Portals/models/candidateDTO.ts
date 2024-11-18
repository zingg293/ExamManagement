export interface CandidateDTO {
  id: string;
  name: string;
  sex: boolean;
  birthday?: Date;
  phone: string;
  email: string;
  address: string;
  idCity?: string;
  idDistrict?: string;
  idWard?: string;
  note: string;
  avatar: string;
  status: number;
  createdDate: Date;
  file: string;
}
