import WithErrorBoundaryCustom from "~/units/errorBounDary/WithErrorBoundaryCustom";
import React, { useEffect, useState } from "react";
import "braft-editor/dist/index.css";
import BraftEditor from "braft-editor";
import { uploadImageToServerFn } from "@admin/components/BraftEditor/PlugIn/uploadImageToServerFn";

interface Props {
  setHtmlContent: (html: string) => void;
  language: "tr" | "zh" | "zh-hant" | "en" | "ru" | "jpn" | "kr" | "pl" | "fr" | "vi-vn";
  initHtmlContent?: string;
  TypeUseMyUploadFn?: number;
}

const defaultType: Props = {
  setHtmlContent: () => void 0,
  language: "en",
  initHtmlContent: `<p>hello <b>world!</b></p>`,
  TypeUseMyUploadFn: 0
};
const _BraftRichEditor: React.FC<Props> = ({ setHtmlContent, language, initHtmlContent, TypeUseMyUploadFn }) => {
  const [state, setState] = useState({ editorState: BraftEditor.createEditorState(initHtmlContent) });
  const handleChange = (editorState: any) => {
    setState({ editorState });
  };
  useEffect(() => {
    setState({ editorState: BraftEditor.createEditorState(initHtmlContent) });
  }, [initHtmlContent]);
  useEffect(() => {
    setHtmlContent(state.editorState.toHTML());
  }, [state, setHtmlContent]);

  return (
    <div className="BraftEditor">
      <BraftEditor
        value={state.editorState}
        onChange={handleChange}
        language={language}
        media={{
          uploadFn: TypeUseMyUploadFn === 1 ? uploadImageToServerFn : undefined
        }}
      />
    </div>
  );
};
_BraftRichEditor.defaultProps = defaultType;
/**
 * @param {setHtmlContent(html: string) => void}setHtmlContent - one function to get HTML content from BraftRichEditor.
 * @param {string}language - language of editor
 * @param {string}initHtmlContent - initial HTML content.
 * @param {number}TypeUseMyUploadFn - 0: use default uploadFn of BraftEditor, 1: use uploadFn of BraftEditor to upload image to server.
 * @returns {JSX.Element}
 * @description - BraftRichEditor is a components to edit HTML content.
 * @author khanhdoan693@gmail.com
 * @example
 * <BraftRichEditor
 * setHtmlContent={setHtmlContent}
 * language="en"
 * initHtmlContent={initHtmlContent}
 * />
 * @version 1.0.0
 */
export const MyEditor = WithErrorBoundaryCustom(_BraftRichEditor);
